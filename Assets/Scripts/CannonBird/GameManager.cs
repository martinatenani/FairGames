using TMPro;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CannonBird
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] TMP_Text energyText;
        [SerializeField] GameObject bullet;
        [SerializeField] float destroyBulletAfterDelay = 1;
        [SerializeField] Transform fireTransform;
        [SerializeField] float fireMaxForce = 500f;
        [SerializeField] private float fireIncreaseSpeed = 50;
        
        private float _fireForce = 0;

        [SerializeField] private ForceMode fireForceMode = ForceMode.Impulse;
        
        [SerializeField] GameObject brick;
        private int _numBricksOnScene = 0;
        
        [SerializeField] Texture2D[] maps;
        private Texture2D _currentMap;
        [SerializeField] private GameObject cannon;
        [SerializeField] private GameObject cannonParent;
        //[SerializeField] private Vector3 startPosition;
        [SerializeField] private Transform startPosition;
        [SerializeField] float deltaX = 2;
        [SerializeField] float deltaY = 2;
        [SerializeField] GameObject boomImage;
        
        [SerializeField] float hSpeed = 30;
        [SerializeField] float vSpeed = 30;

        private Vector2 _rotationValues;
        
        [SerializeField] float minVerticalAngle = -10f;
        [SerializeField] float maxVerticalAngle = 60f;
        
        [SerializeField] float minHorizontalAngle = -90f;
        [SerializeField] float maxHorizontalAngle = 90f;

        private float _verticalAngle;
        private float _horizontalAngle;
        private Quaternion _cannonParentRotation;

        [SerializeField] bool debugThrow = true;
        
        //Input actions
        private InputSystem_Actions _inputActions;
        private InputAction _fireAction;
        private InputAction _quitAction;
        
        private void Awake()
        {
            _inputActions = new InputSystem_Actions();
        }
        
        private void OnEnable()
        {
            _inputActions.Enable();
            _inputActions.UI.Quit.performed += OnQuit;
            BrickManager.OnDestroyed += HandleOnBrickDestroyed;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _cannonParentRotation = cannonParent.transform.rotation;
            
            _fireAction = InputSystem.actions.FindAction("Attack");
            _quitAction = InputSystem.actions.FindAction("Quit");
            
            _currentMap = maps[Random.Range(0,maps.Length)];
            
            for (int i = 0; i < _currentMap.width; i++)
            {
                for (int j = 0; j < _currentMap.height; j++)
                {
                    if (_currentMap.GetPixel(i, j).r == 0)
                    {
                        Debug.Log($"{_currentMap.GetPixel(i, j)} found...");
                        GameObject go = Instantiate(brick);
                        go.GetComponent<Rigidbody>().isKinematic = true;
                        go.transform.rotation = startPosition.rotation;
                        
                        Vector3 offset =
                            startPosition.right * (i * deltaX) +
                            startPosition.up * (j * deltaY);

                        go.transform.position = startPosition.position + offset;

                        _numBricksOnScene++;
                    }
                }
            }
        }

        private void OnMove(InputValue context)
        {
            _rotationValues = context.Get<Vector2>();
            Debug.Log($"{_rotationValues}");
        }

        void Update()
        {
            _verticalAngle += _rotationValues.y *hSpeed *Time.deltaTime;
            _horizontalAngle += _rotationValues.x *vSpeed * Time.deltaTime;
            
            _verticalAngle = Mathf.Clamp(_verticalAngle, minVerticalAngle, maxVerticalAngle);
            _horizontalAngle = Mathf.Clamp(_horizontalAngle, minHorizontalAngle, maxHorizontalAngle);
            
            cannon.transform.localRotation = Quaternion.Euler(0f,_verticalAngle,0f);
            cannonParent.transform.rotation = _cannonParentRotation*Quaternion.Euler(0f,0f,_horizontalAngle);

            if (_fireAction.WasPressedThisFrame())
            {
                _fireForce = 0;
                energyText.text = "";
            }else if (_fireAction.IsPressed())
            {
                _fireForce = Mathf.Clamp(_fireForce + Time.deltaTime * fireIncreaseSpeed,0,_fireForce + Time.deltaTime * fireIncreaseSpeed);
                energyText.text = $"Energy:{(int)_fireForce}";
            }else if (_fireAction.WasReleasedThisFrame())
            {
                GameObject go = Instantiate(bullet);
                go.SetActive(true);
                go.transform.position = fireTransform.position;
                go.transform.rotation = _cannonParentRotation;

                if (debugThrow) Debug.Break(); //pause the game
                
                Rigidbody rb = go.GetComponent<Rigidbody>();
                rb.MovePosition(fireTransform.position);
                rb.MoveRotation(fireTransform.rotation);
                
                rb.AddRelativeForce(fireTransform.forward.normalized * _fireForce, fireForceMode);
                rb.AddRelativeTorque(Vector3.right * 50);
                
                Destroy(go,destroyBulletAfterDelay);
                
                boomImage.SetActive(true);
                energyText.text = "";
            }
        }
        
        private void HandleOnBrickDestroyed()
        {
            if (_numBricksOnScene <1)
            {
                Debug.Log("Game over");
            }
            _numBricksOnScene--;
        }
        
        private void OnDisable()
        {
            _inputActions.UI.Quit.performed -= OnQuit;
            _inputActions.Disable();
        }
        
        private void OnQuit(InputAction.CallbackContext context)
        {
            QuitGame();
        }
    
        private void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}
