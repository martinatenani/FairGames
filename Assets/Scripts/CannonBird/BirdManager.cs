using UnityEngine;
using UnityEngine.LowLevelPhysics;

namespace CannonBird
{
    public class BirdManager : MonoBehaviour
    {
        private Rigidbody _rb;
        
        [Header("Audio")]
        [SerializeField] private AudioClip[] spawnSounds;
        [SerializeField] private AudioClip[] destroySounds;
        
        [Header("Physics booster")]
        [SerializeField] [Range(0,30)] float gravityMultiplier = 5;

        private static readonly Vector3 Gravity = Physics.gravity;
        
        [Header("visual feedbacks")]
        [SerializeField] private GameObject boomObject;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            
            GetComponent<AudioSource>().clip = spawnSounds[Random.Range(0, spawnSounds.Length)]; //impostazione del suono in modo casuale
            GetComponent<AudioSource>().Play();
            
            boomObject.SetActive(false);
        }

        void FixedUpdate()
        {
            _rb.AddForce(Gravity * (_rb.mass * gravityMultiplier)); // f = ma
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.CompareTag("Brick"))
            {
                GameObject go = Instantiate(boomObject, this.transform.position, Quaternion.identity);
                go.SetActive(true);
                go.transform.LookAt(Camera.main!.transform); //non è clampato sulle rotazioni -> può ruotare tutti gli assi. (Si potrebbe calcolare la direzione azzerando le rotazioni)
                AudioSource.PlayClipAtPoint(destroySounds[Random.Range(0,destroySounds.Length)], collision.transform.position);
            }
        }
    }
}
