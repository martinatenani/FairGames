using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private int collisions = 0;

    private void OnCollisionEnter(Collision collision)
    {
        collisions++;
        Debug.Log($"Collisions:{collisions}");
    }

    private void OnCollisionExit(Collision collision)
    {
        collisions--;
        Debug.Log($"Collisions:{collisions}");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
