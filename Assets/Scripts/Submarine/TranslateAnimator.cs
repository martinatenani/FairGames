using System;
using UnityEngine;

namespace Submarine
{
    public class TranslateAnimator : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private Vector3 direction = Vector3.right;

        private void Start()
        {
            direction = direction.normalized * speed;
        }

        void Update()
        {
            transform.Translate(direction * Time.deltaTime);
        }
    }
}
