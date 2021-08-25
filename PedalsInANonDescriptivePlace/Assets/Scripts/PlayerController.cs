using UnityEngine;

namespace PedalsInANonDescriptivePlace
{
    [RequireComponent(typeof(Rigidbody))]    
    public class PlayerController : MonoBehaviour
    {
        private const string PEDAL_AXIS = "PedalInput";
        private const string HORIZONTAL_STEER = "Horizontalzzz";
        private const string BRAKE = "Fire2";
        private const string ACCELERATE = "Fire3";

        private const float EPSILON = 0f;
        
        [SerializeField] private Vector3 _pedalForce = 5f * Vector3.up;
        [SerializeField] private Vector3 _steeringSpeed = 1f * Vector3.up;
        [SerializeField] private float _maxPedalVelocity = 4f;
        [SerializeField] private Animation _animation;
        [SerializeField] private float _animationSpeed = 1;
        [SerializeField] private float _defaultHorizontalVelocity;
        [SerializeField] private float _maxHorizontalVelocity;

        private float _velocityHorizontalSpeed;
        private Rigidbody _rigidbody;
        private bool isSoaring;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _velocityHorizontalSpeed = _defaultHorizontalVelocity;
            _rigidbody.velocity = transform.forward * _defaultHorizontalVelocity;
        }

        private void FixedUpdate()
        {
            UpdatePedalForce();
            UpdateSteering();
            UpdateForwardVelocity();
        }

        private void UpdatePedalForce()
        {
            float pedalPower = Input.GetAxis(PEDAL_AXIS);
            // After reaching certain velocity, adding more force towards the same direction is not possible 
            if (Mathf.Sign(_rigidbody.velocity.y) == Mathf.Sign(pedalPower)
                && Mathf.Abs(_rigidbody.velocity.y) > _maxPedalVelocity)
            {
                if (!isSoaring)
                {
                    _animation.CrossFade("ToonSeagullSoar");
                    isSoaring = true;
                }
                return;
            }
        
            if (pedalPower > 0f)
            {
                _rigidbody.AddForce(_pedalForce * pedalPower);
                if (isSoaring)
                {
                    _animation.CrossFade("ToonSeagullFlap");
                    isSoaring = false;
                }
                _animation["ToonSeagullFlap"].speed = pedalPower * _animationSpeed;
                
            }
            else
            {
                if (!isSoaring)
                {
                    _animation.CrossFade("ToonSeagullSoar");
                    isSoaring = true;
                }
            }
        }

        private void UpdateSteering()
        {
            float steer = Input.GetAxis(HORIZONTAL_STEER);
            transform.Rotate(_steeringSpeed * steer);
        }

        private void UpdateForwardVelocity()
        {
            float verticalVelocity = _rigidbody.velocity.y;
            float targetSpeed;
            if (Input.GetButton(ACCELERATE))
                targetSpeed = _maxHorizontalVelocity;
            else if (Input.GetButton(BRAKE))
                targetSpeed = 0f;
            else
                targetSpeed = _defaultHorizontalVelocity;

            _velocityHorizontalSpeed = Mathf.Lerp(_velocityHorizontalSpeed, targetSpeed, 0.5f * Time.deltaTime);

            _rigidbody.velocity = transform.forward * _velocityHorizontalSpeed;
            Vector3 tempVelocity = _rigidbody.velocity;
            tempVelocity.y = verticalVelocity;
            _rigidbody.velocity = tempVelocity;
            
            var localVelocity = transform.InverseTransformDirection(_rigidbody.velocity);
            Quaternion birdRotation = Quaternion.Euler(-localVelocity.y * 5, transform.eulerAngles.y, transform.eulerAngles.z);
            transform.GetChild(0).transform.rotation = Quaternion.Lerp(transform.GetChild(0).transform.rotation, birdRotation, 5 * Time.deltaTime);
            
        }
    }
}