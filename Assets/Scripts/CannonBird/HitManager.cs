using UnityEngine;

namespace CannonBird
{
    public class HitManager : MonoBehaviour
    {
        [SerializeField] private Sprite[] texts;
        [SerializeField] private float fadeSpeed = 1f;
        [SerializeField] private float liftSpeed = 1f;

        private SpriteRenderer _sr;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _sr = GetComponent<SpriteRenderer>();
            _sr.sprite = texts[Random.Range(0, texts.Length)];
            Destroy(gameObject, fadeSpeed);
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.up * (liftSpeed * Time.deltaTime));
            var c = _sr.color;
            c.a -= fadeSpeed * Time.deltaTime;
        
            _sr.color = c;
        }
    }
}
