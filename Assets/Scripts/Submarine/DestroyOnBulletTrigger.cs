using UnityEngine;
using Random = UnityEngine.Random;

namespace Submarine
{
    public class DestroyOnBulletTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        [SerializeField] private GameObject spawnOnDestroy;
        [SerializeField] [Range(1,10)] private int minInstances = 3;
        [SerializeField] [Range(1,10)] private int maxInstances = 10;
        [SerializeField] private Vector3 randomBubbleDelta = Vector3.zero;

        private const float DESTROY_DELAY = 3f;
        private void OnTriggerEnter(Collider other)
        {
            if (spawnOnDestroy)
            {
                int realInstances = minInstances + Random.Range(0, maxInstances + 1);

                for (int i = 0; i < realInstances; i++)
                {
                    GameObject go = Instantiate (spawnOnDestroy,
                        transform.position + randomBubbleDelta*Random.Range(-1f,1f),
                                spawnOnDestroy.transform.rotation);
                    go.transform.localScale = Vector3.one * Random.Range(.5f, 1.5f);
                    Destroy (go, DESTROY_DELAY);
                }
            }
            Destroy(target?target:gameObject);
        }
    }
}
