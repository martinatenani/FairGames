using UnityEngine;

namespace CannonBird
{
    public class PrefabManager : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int elements = 10;
        [SerializeField] private float startDelay = 0;
        [SerializeField] private float repeatRate = 1;

        private int _counter = 0;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            InvokeRepeating(nameof (PlacePrefab),startDelay,repeatRate);
        }

        void PlacePrefab()
        {
            if (_counter >= elements)
            {
                CancelInvoke(nameof(PlacePrefab));
                return;
            }

            _counter++;
            
            GameObject go = Instantiate(prefab);
            go.transform.position = Random.onUnitSphere * 5;
            go. transform.rotation = Random.rotation;
        }

    }
}
