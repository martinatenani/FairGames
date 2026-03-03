using UnityEngine;

public class CheckSimilarCollision : MonoBehaviour
{
    private string _myName;
    public static event System.Action<GameObject, GameObject> OnSimilarCollided;
    
    void OnEnable()
    {
        _myName = this.gameObject.name;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == _myName)
        {
            OnSimilarCollided?.Invoke(this.gameObject, collision.gameObject);
        }
    }

}
