using System;
using UnityEngine;
using UnityEngine.Animations;
using Object = UnityEngine.Object;

namespace CannonBird
{
    public class BrickManager : MonoBehaviour
    {
        public static event System.Action OnDestroyed;
        private bool floatAway = false;
        private GameObject _explosionPrefab;

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
            }else if (collision.gameObject.CompareTag("Bomb"))
            {
                Collider[] hits = Physics.OverlapBox(transform.position + new Vector3(0, 5, 0),
                    new Vector3(30f, 30f, .30f),
                    Quaternion.identity);
                foreach (Collider c in hits)
                {
                    if (c.TryGetComponent(out Rigidbody rb))
                    {
                        rb.isKinematic = false;
                        rb.AddExplosionForce(700f,collision.transform.position,100f);
                    } //to avoid using layers
                }
                //Destroy(gameObject);
            }else if(collision.gameObject.CompareTag("Shrink"))
            {
                Collider[] hits = Physics.OverlapBox(transform.position + new Vector3(0, 5, 0),
                    new Vector3(.5f, 10f, .5f),
                    Quaternion.identity);
                foreach (Collider c in hits)
                {
                    if (c.TryGetComponent(out Rigidbody rb))
                    {
                        rb.isKinematic = true;
                    } //to avoid using layers
                }
                gameObject.transform.localScale += new Vector3(-0.1f,-0.1f,-0.1f);

                if (gameObject.transform.localScale.x < 0.05f)
                {
                    Destroy(gameObject);
                }
            }else if(collision.gameObject.CompareTag("Float"))
            {
                Collider[] hits = Physics.OverlapBox(transform.position + new Vector3(0, 5, 0),
                    new Vector3(.5f, 10f, .5f),
                    Quaternion.identity);
                
                foreach (Collider c in hits)
                {
                    if (c.TryGetComponent(out Rigidbody rb))
                    {
                        rb.isKinematic = true;
                        floatAway = true;
                    } //to avoid using layers
                }
            }
        }

        private void Update()
        {
            if (floatAway)
            {
                transform.position += new Vector3(0,1,0) * 0.001f;
            }
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
        }
    }
}
