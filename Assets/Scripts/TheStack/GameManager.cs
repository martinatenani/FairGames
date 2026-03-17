using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    private int MAX_NUMBER_OF_MERGES = 4;
    
    private InputSystem_Actions _inputActions;

    private int _currentPoints = 0;
    [FormerlySerializedAs("_pointTrackerText")] [SerializeField]private TMPro.TextMeshPro pointTrackerText;
    
    private SBlockStats _primitiveToPlace;
    private GameObject _previewObject;
    private GameObject _placedPrimitive;
    private List<GameObject> _placedOnTable = new List<GameObject>();
    
    private Vector3 _nextShapePreviewPos = new Vector3(-15, 1, 1);

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.UI.Quit.performed += OnQuit;

        CollisionDetector.OnTableCollided += HandleOnTableCollided;
        CheckSimilarCollision.OnSimilarCollided += HandleOnSimilarCollided;
        DestroyOnFall.OnFall += HandleOnFall;
        
        pointTrackerText.text = "0";
    }
    
    private void HandleOnTableCollided(GameObject obj)
    {
        if (_placedPrimitive != null)
        {
            if (!_placedPrimitive.GetComponent<BlockStats>().OnTable)
            {
                Debug.Log("OnTableCollided");
                _placedPrimitive.GetComponent<BlockStats>().OnTable = true;
                _placedOnTable.Add(_placedPrimitive);
                _currentPoints += _placedPrimitive.GetComponent<BlockStats>().blockStats.Value;
            }
        }
        
        pointTrackerText.text  = $"{_currentPoints}";
    }
    
    private void HandleOnFall(GameObject obj)
    {
        _currentPoints -= obj.GetComponent<BlockStats>().blockStats.Value;
        _placedOnTable.Remove(obj);
        pointTrackerText.text  = $"{_currentPoints}";
    }

    private void HandleOnSimilarCollided(GameObject obj1, GameObject obj2)
    {
        GameObject biggerObj = null;
        GameObject smallerObj = null;
        //randomly merge the two
        if (obj1.GetComponent<BlockStats>().blockStats.Value >= obj2.GetComponent<BlockStats>().blockStats.Value)
        {
            biggerObj = obj1;
            smallerObj = obj2;
        }
        else
        {
            biggerObj = obj2;
            smallerObj = obj1;
        }
        biggerObj.GetComponent<BlockStats>().blockStats.Value += smallerObj.GetComponent<BlockStats>().blockStats.Value; //add the value of the smaller one to the bigger one;
        Debug.Log("Object value after merge: " + biggerObj.GetComponent<BlockStats>().blockStats.Value);
        biggerObj.GetComponent<Transform>().position = 
            new Vector3(
                (biggerObj.transform.position.x + smallerObj.transform.position.x)*0.5f, 
                (biggerObj.transform.position.y + smallerObj.transform.position.y)*0.5f, 
                (biggerObj.transform.position.z + smallerObj.transform.position.z)*0.5f
            );
        biggerObj.GetComponent<Transform>().localScale *= 1.2f;
        biggerObj.GetComponent<MeshRenderer>().material.color = GetRandomColorForPrimitive(); //change color
        biggerObj.GetComponent<BlockStats>().Merges++;
        
        _placedOnTable.Remove(smallerObj); //remove this one from the list
        Destroy(smallerObj); //destroy the second one

        if (biggerObj.GetComponent<BlockStats>().Merges > MAX_NUMBER_OF_MERGES)
        {
            _placedOnTable.Remove(biggerObj); //remove this one from the list
            Destroy(biggerObj); //destroy the second one
        }
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
            case 0:
                _primitiveToPlace = new SBlockStats(PrimitiveType.Cube, 5);
                break;
            case 1:
                _primitiveToPlace = new SBlockStats(PrimitiveType.Sphere, 30);
                break;
            case 2:
                _primitiveToPlace = new SBlockStats(PrimitiveType.Capsule, 20);
                break;
            case 3:
                _primitiveToPlace = new SBlockStats(PrimitiveType.Cylinder, 10);
                break;
            default: 
                _primitiveToPlace = new SBlockStats(PrimitiveType.Cube, 5);
                break;
        }
        if(_previewObject)Destroy(_previewObject);
        _previewObject = GameObject.CreatePrimitive(_primitiveToPlace.PrimitiveType);
        _previewObject.name = "Preview shape";
        _previewObject.transform.position = _nextShapePreviewPos;
    }

    private Color GetRandomColorForPrimitive()
    {
        //Control randomness
        Color randomColor = Random.ColorHSV();

        float H, S, V;
        Color.RGBToHSV(randomColor, out H, out S, out V);
        S = .8f;
        V = .8f;

        randomColor = Color.HSVToRGB(H, S, V);
        return randomColor;
    }
    
    private void CreateNewBlock(SBlockStats blockStats, RaycastHit hit)
    {
        _placedPrimitive = GameObject.CreatePrimitive(_primitiveToPlace.PrimitiveType);
    
        _placedPrimitive.name = $"{_primitiveToPlace.PrimitiveType.ToString()}";
        _placedPrimitive.transform.localScale = Vector3.one * 0.3f;
        _placedPrimitive.transform.position = hit.point + new Vector3(0, 1f, 0);
        _placedPrimitive.transform.rotation = Random.rotation;
                
        _placedPrimitive.AddComponent<Rigidbody>();
        _placedPrimitive.GetComponent<MeshRenderer>().material.color = GetRandomColorForPrimitive();
                    
        //load a texture from assets
        //Texture texture = Resources.Load<Texture>("Textures/wood_texture");
        //go.GetComponent<MeshRenderer>().material.mainTexture = texture;
        var statsRef = _placedPrimitive.AddComponent<BlockStats>();
        statsRef.blockStats = new SBlockStats(_primitiveToPlace.PrimitiveType, _primitiveToPlace.Value);
        
        _placedPrimitive.AddComponent<DestroyOnFall>();
        _placedPrimitive.AddComponent<DragWithMouse>();
        _placedPrimitive.AddComponent<CheckSimilarCollision>();
        _placedPrimitive.AddComponent<CollisionDetector>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) //right click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                CreateNewBlock(_primitiveToPlace, hit);
                GenerateNextShape();
            }
        }
    }
}
