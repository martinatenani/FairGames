using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private int collisions = 0;
    public static event System.Action<GameObject> OnTableCollided;
    
    private void OnCollisionEnter(Collision collision)
    {
        collisions++;
        OnTableCollided?.Invoke(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        collisions--;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
