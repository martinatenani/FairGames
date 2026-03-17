using System;
using UnityEngine;

namespace CannonBird
{
    public class BrickManager : MonoBehaviour
    {
        public static event System.Action OnDestroyed;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                Collider[] hits = Physics.OverlapBox(transform.position+new Vector3(0,5,0),
                new Vector3(.5f,10f,.5f),
                Quaternion.identity);

                foreach (Collider c in hits)
                {
                    if (c.TryGetComponent(out Rigidbody rb))
                    {
                        rb.isKinematic = false;
                    } //to avoid using layers
                }
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
        }
    }
}
