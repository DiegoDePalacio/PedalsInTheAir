using UnityEngine;

namespace PedalsInANonDescriptivePlace
{
    [RequireComponent(typeof(Rigidbody))]    
    public class PlayerController : MonoBehaviour
    {
        private const string PEDAL_AXIS = "PedalInput";
        private const string HORIZONTAL_STEER = "Horizontalzzz";
        
        // private const string STEER_LEFT = "SteerLeft";
        // private const string STEER_RIGHT = "SteerRight";

        private const float EPSILON = 0f;
        
        [SerializeField] private Vector3 _pedalForce = 5f * Vector3.up;
        [SerializeField] private Vector3 _steeringSpeed = 1f * Vector3.up;
        [SerializeField] private float _maxPedalVelocity = 4f;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            UpdatePedalForce();
            UpdateSteering();
        }

        private void UpdatePedalForce()
        {
            float pedalPower = Input.GetAxis(PEDAL_AXIS);
            
            // After reaching certain velocity, adding more force towards the same direction is not possible 
            if (Mathf.Sign(_rigidbody.velocity.y) == Mathf.Sign(pedalPower)
                && Mathf.Abs(_rigidbody.velocity.sqrMagnitude) > _maxPedalVelocity)
            {
                return;
            }
        
            if (pedalPower > 0f)
            {
                _rigidbody.AddForce(_pedalForce * pedalPower);
            }
        }

        private void UpdateSteering()
        {
            float steer = Input.GetAxis(HORIZONTAL_STEER);
            transform.Rotate(_steeringSpeed * steer);
        }

        // private void UpdateSteeringAlternative()
        // {
        //     float steerLeft = Input.GetAxis(STEER_LEFT);
        //
        //     if (steerLeft > 0f)
        //     {
        //         transform.Rotate(_steeringSpeed * -steerLeft);
        //     }
        //     
        //     float steerRight = Input.GetAxis(STEER_RIGHT);
        //
        //     if (steerRight > 0f)
        //     {
        //         transform.Rotate(_steeringSpeed * steerRight);
        //     }   
        // }
    }
}