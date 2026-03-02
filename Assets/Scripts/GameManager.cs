using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    
    private InputSystem_Actions _inputActions;
    
    private PrimitiveType _primitiveToPlace;
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
            case 0: _primitiveToPlace = PrimitiveType.Cube; break;
            case 1: _primitiveToPlace = PrimitiveType.Sphere; break;
            case 2: _primitiveToPlace = PrimitiveType.Capsule; break;
            case 3: _primitiveToPlace = PrimitiveType.Cylinder; break;
            default: _primitiveToPlace = PrimitiveType.Cube; break;
        }
        if(_previewObject)Destroy(_previewObject);
        _previewObject = GameObject.CreatePrimitive(_primitiveToPlace);
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
                GameObject go = GameObject.CreatePrimitive(_primitiveToPlace);
                
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
