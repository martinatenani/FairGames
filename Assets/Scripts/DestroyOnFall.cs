using UnityEngine;

public class DestroyOnFall : MonoBehaviour
{
    // Update is called once per frame
    void Update() //renderlo un controllo ogni X MS/SECONDI o ogni X FOTOGRAMMI
    //Gestrire la distruzione in un modo più efficiente, ad esempio usando un trigger collider sotto la scena che distrugge gli oggetti che lo attraversano
    //
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
