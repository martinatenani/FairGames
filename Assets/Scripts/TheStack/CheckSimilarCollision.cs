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
            if(GetInstanceID()< collision.gameObject.GetInstanceID()) //evitare che si chiamino a vicenda
                OnSimilarCollided?.Invoke(this.gameObject, collision.gameObject);
        }
    }

}
