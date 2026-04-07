using UnityEngine;
using UnityEngine.InputSystem;

namespace Submarine
{
    public class FireManager : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float firePower = 30;
        [SerializeField] private float fireRate = 1;
        [SerializeField] private ForceMode fireMode = ForceMode.Impulse;
        [SerializeField] private Transform[] firePositions;
        [SerializeField] private Transform root;

        private float _fireTimer = 0;
        private InputAction _attackAction;
        private float BULLET_SUTODESTROY_DELAY = 5f;
        private void Start()
        {
            _attackAction = InputSystem.actions.FindAction("Attack");
        }

        private void Update()
        {
            _fireTimer += Time.deltaTime;
            if (_fireTimer < fireRate) return;
            if (_attackAction.IsPressed())
            {
                for (int i = 0; i < firePositions.Length; i++)
                {
                    GameObject bullet = Instantiate(bulletPrefab, firePositions[i].position, bulletPrefab.transform.rotation*root.rotation);
                    Rigidbody rb = bullet.GetComponent<Rigidbody>();
                    rb.AddRelativeForce(Vector3.down * firePower, fireMode);
                    Destroy(bullet, BULLET_SUTODESTROY_DELAY);
                }

                _fireTimer = 0;
            }
        }
    }
}
