using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public struct Block
{
    public PrimitiveType PrimitiveType { get; set; }
    public int Value { get; set; }
}

public class GameManager : MonoBehaviour
{
    
    private InputSystem_Actions _inputActions;

    private int _currentPoints = 0;
    [SerializeField]private TMPro.TextMeshPro _pointTrackerText;
    
    private Block _primitiveToPlace;
    private Block _placedPrimitive;
    
    private Vector3 _nextShapePreviewPos = new Vector3(-7, 1, 1);
    private GameObject _previewObject;

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.UI.Quit.performed += OnQuit;

        CollisionDetector.OnTableCollided += HandleOnTableCollided;
        CollisionDetector.OnTableUncollided += HandleOnTableUncollided;
        
        _pointTrackerText.text = "0";
    }

    private void HandleOnTableCollided(GameObject obj)
    {
        _currentPoints += _placedPrimitive.Value;
        _pointTrackerText.text  = $"{_currentPoints}";
    }
    
    private void HandleOnTableUncollided(GameObject obj)
    {
        _currentPoints -= _placedPrimitive.Value;
        _pointTrackerText.text  = $"{_currentPoints}";
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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateNextShape();
    }
    
    private void GenerateNextShape()
    {
        switch (Random.Range(0, 4))
        {
            case 0: _primitiveToPlace = new Block
            {
                PrimitiveType = PrimitiveType.Cube,
                Value = 5
            };
            break;
            case 1: _primitiveToPlace = new Block 
            {
                PrimitiveType = PrimitiveType.Sphere,
                Value = 30
            };
            break;
            case 2: _primitiveToPlace = new Block 
            {
                PrimitiveType = PrimitiveType.Capsule,
                Value = 20
            };
            break;
            case 3: _primitiveToPlace = new Block 
            {
                PrimitiveType = PrimitiveType.Cylinder,
                Value = 10
            };
            break;
            default: _primitiveToPlace = new Block
            {
                PrimitiveType = PrimitiveType.Cube,
                Value = 5
            };
            break;
        }
        if(_previewObject)Destroy(_previewObject);
        _previewObject = GameObject.CreatePrimitive(_primitiveToPlace.PrimitiveType);
        _previewObject.name = "Preview shape";
        _previewObject.transform.position = _nextShapePreviewPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) //right click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                _placedPrimitive = new Block
                {
                    PrimitiveType = _primitiveToPlace.PrimitiveType,
                    Value = _primitiveToPlace.Value
                };
                
                GameObject go = GameObject.CreatePrimitive(_placedPrimitive.PrimitiveType);
                go.transform.localScale = Vector3.one * 0.3f;
                go.transform.position = hit.point + new Vector3(0, 1f, 0);
                go.transform.rotation = Random.rotation;
                
                go.AddComponent<Rigidbody>();
                
                //Control randomness
                    Color randomColor = Random.ColorHSV();

                    float H, S, V;
                    Color.RGBToHSV(randomColor, out H, out S, out V);
                    S = .8f;
                    V = .8f;

                    randomColor = Color.HSVToRGB(H, S, V);
                    go.GetComponent<MeshRenderer>().material.color = randomColor;
                    
                //load a texture from assets
                //Texture texture = Resources.Load<Texture>("Textures/wood_texture");
                //go.GetComponent<MeshRenderer>().material.mainTexture = texture;
                
                go.AddComponent<DestroyOnFall>();
                go.AddComponent<DragWithMouse>();
                GenerateNextShape();
            }
        }
    }
}
