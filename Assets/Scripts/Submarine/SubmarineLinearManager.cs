using UnityEngine;
using UnityEngine.InputSystem;

namespace Submarine
{
    public class SubmarineLinearManager : MonoBehaviour
    {
        [SerializeField] private float fuel = 100f;
        [SerializeField] private float maxFuel = 100f;
        [SerializeField] private float fuelUsageSpeed = 1f;
        [SerializeField] private float mineFuelReduction = 5f;
        [SerializeField] private float boxFuelIncrement = 5f;

        [SerializeField] private Vector3 impulseForce = Vector3.up * 0;
        [SerializeField] private Vector3 constantForce = Vector3.up * 0;
        [SerializeField] private Vector3 forwardForce = Vector3.left * 10f;
        [SerializeField] private ForceMode forceMode = ForceMode.Force;
    
        [SerializeField] private float minRotation = 35f;
        [SerializeField] private float maxRotation = -35f;
        [SerializeField] private float pitchSpeed = 1;
        [SerializeField] private float speed = 1;
        [SerializeField] private Transform ship;

        private bool _thrust;
        private Rigidbody _rb;
        private InputAction _jumpAction;
        private FuelStatusViewer _fuelStatusViewer;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _jumpAction = InputSystem.actions.FindAction("Jump");
            _fuelStatusViewer = GetComponent<FuelStatusViewer>();
            _fuelStatusViewer.SetFuel(fuel/maxFuel);
        
            InvokeRepeating(nameof(UpdateFuel), 0, 1);
        }

        private void UpdateFuel()
        {
            _fuelStatusViewer.SetFuel(fuel/maxFuel);
        }

        void Update()
        {
            fuel -= Time.deltaTime*fuelUsageSpeed;
            if (fuel <= 0) //end of fuel
            {
                enabled = false;
                return;
            }

            if (_jumpAction.WasPressedThisFrame())
            {
                _thrust = true;
            }else if (_jumpAction.IsPressed())
            {
                Vector3 dest = new Vector3(maxRotation, ship.transform.localRotation.eulerAngles.y, ship.transform.localRotation.eulerAngles.z);
                ship.transform.localRotation = Quaternion.Lerp(ship.transform.localRotation, Quaternion.Euler(dest), Time.deltaTime * fuelUsageSpeed);
            }else if (_jumpAction.WasReleasedThisFrame())
            {
                _thrust = false;
            }
            else
            {
                Vector3 dest = new Vector3(minRotation, ship.transform.localRotation.eulerAngles.y, -ship.transform.localRotation.eulerAngles.z);
                ship.transform.localRotation = Quaternion.Lerp(ship.transform.localRotation, Quaternion.Euler(dest), Time.deltaTime * pitchSpeed);
            }
        }

        private void FixedUpdate()
        {
            if (_thrust)
            {
                if (forceMode == ForceMode.Impulse)
                {
                    _thrust = false;
                    _rb.AddForce(impulseForce, forceMode);
                    ship.transform.localEulerAngles = new Vector3(maxRotation, ship.transform.localRotation.eulerAngles.y, ship.transform.localRotation.eulerAngles.z);
                }
                else
                {
                    _rb.AddForce(constantForce, forceMode);
                }
            }
            //_rb.AddForce(-forwardForce, ForceMode.Force);
            _rb.MovePosition(_rb.position + Vector3.right * (10f * Time.fixedDeltaTime));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Box"))
            {
                Destroy(other.gameObject);
                fuel = Mathf.Clamp(fuel + boxFuelIncrement, 0, maxFuel);
                UpdateFuel();
            }
            if (other.gameObject.CompareTag("Mine"))
            {
                Destroy(other.gameObject);
                fuel = Mathf.Clamp(fuel - mineFuelReduction, 0, maxFuel);
            
                if (fuel <= 0)
                {
                    enabled = false;
                }
            
                UpdateFuel();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            _rb.isKinematic = true;
            enabled = false;

            fuel = -1;
            UpdateFuel();
        
            Destroy(GetComponent<FireManager>());
        
            CancelInvoke();
            Destroy(this);
        }
    }
}
