using UnityEngine;

public class DestroyOnFall : MonoBehaviour
{
    public static event System.Action<GameObject> OnFall;
    // Update is called once per frame
    void Update() //renderlo un controllo ogni X MS/SECONDI o ogni X FOTOGRAMMI
    //Gestrire la distruzione in un modo più efficiente, ad esempio usando un trigger collider sotto la scena che distrugge gli oggetti che lo attraversano
    //
    {
        if (transform.position.y < -10)
        {
            OnFall?.Invoke(this.gameObject);
            Destroy(gameObject);
        }
    }
}
