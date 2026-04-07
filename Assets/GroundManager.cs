using System.Linq;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    [SerializeField] private GameObject[] GroundChunks;
    [SerializeField] private float GROUND_OFFSET = 200f;

    private float cameraHalfWidth;
    private float cameraLeftEdge;
    
    private BoxCollider groundExtent;

    void Start()
    {
        cameraLeftEdge = Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect);
    }
    
    // Update is called once per frame
    void Update()
    {
        cameraHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        cameraLeftEdge = Camera.main.transform.position.x - cameraHalfWidth;

        foreach (GameObject chunk in GroundChunks)
        {
            BoxCollider col = chunk.GetComponentInChildren<BoxCollider>();
            if (col == null) continue;

            // Convert collider center to world space
            float chunkRightEdge = col.bounds.max.x;

            if (chunkRightEdge < cameraLeftEdge)
            {
                RepositionChunk(chunk);
            }
        }
    }

    void RepositionChunk(GameObject chunk)
    {
        // Find the rightmost edge among ALL chunks
        float rightMostEdge = GroundChunks
            .Max(c => c.GetComponentInChildren<BoxCollider>().bounds.max.x);

        BoxCollider col = chunk.GetComponentInChildren<BoxCollider>();
        float chunkWidth = col.bounds.size.x;

        chunk.transform.position = new Vector3(
            rightMostEdge + chunkWidth / 2f,
            chunk.transform.position.y,
            chunk.transform.position.z
        );
    }
}
