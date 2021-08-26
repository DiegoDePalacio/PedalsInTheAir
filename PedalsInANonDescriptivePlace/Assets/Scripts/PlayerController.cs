using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace PedalsInANonDescriptivePlace
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        private const string PEDAL_AXIS = "PedalInput";
        private const string HORIZONTAL_STEER = "Horizontalzzz";

        public static KeyCode steerLeftKeyboard = KeyCode.A;
        public static KeyCode steerRightKeyboard = KeyCode.D;
        public static KeyCode ability1Keyboard = KeyCode.Q;
        public static KeyCode ability2Keyboard = KeyCode.E;
        public static KeyCode ability3Keyboard = KeyCode.Space;
        public static KeyCode ability4Keyboard = KeyCode.R;
        public static KeyCode startLeftKeyboard = KeyCode.T;
        public static KeyCode startRightKeyboard = KeyCode.Y;

        public static KeyCode joy0 = KeyCode.JoystickButton1;
        public static KeyCode joy1 = KeyCode.JoystickButton0;
        public static KeyCode joy2 = KeyCode.JoystickButton2;
        public static KeyCode joy3 = KeyCode.JoystickButton3;
        public static KeyCode joy4 = KeyCode.JoystickButton4;
        public static KeyCode joy5 = KeyCode.JoystickButton5;
        public static KeyCode joy6 = KeyCode.JoystickButton6;
        public static KeyCode joy7 = KeyCode.JoystickButton7;

        [SerializeField] private Vector3 _pedalForce = 5f * Vector3.up;
        [SerializeField] private Vector3 _steeringSpeed = 1f * Vector3.up;
        [SerializeField] private float _maxPedalVelocity = 4f;
        [SerializeField] private Animation _animation;
        [SerializeField] private float _animationSpeed = 1;
        [SerializeField] private float _defaultHorizontalVelocity;
        [SerializeField] private float _maxHorizontalVelocity;
        [SerializeField] private int _maxFramesPerAverage;
        [SerializeField] private Slider VerticalForceSlider;
        [SerializeField] private Slider HorizontalMaxVelocitySlider;
        [SerializeField] private Slider MaxAverageFramesSlider;

        private float _velocityHorizontalSpeed;
        private Rigidbody _rigidbody;
        private bool isSoaring;
        private List<float> _lastInputs = new List<float>();
        private int _currentFrame;
        
        // TODO: Move all these to an Input Manager
        private static bool SteerRight => Input.GetKey(steerRightKeyboard) || Input.GetKey(joy3);
        private static bool SteerLeft => Input.GetKey(steerLeftKeyboard) || Input.GetKey(joy2);
        private static bool Accelerate => Input.GetKey(ability1Keyboard) || Input.GetKey(joy1);
        private static bool Breake => Input.GetKey(ability2Keyboard) || Input.GetKey(joy0);

        public static bool ShootDown1 => Input.GetKeyDown(ability3Keyboard) || Input.GetKeyDown(joy4);
        public static bool ShootUp1 => Input.GetKeyUp(ability3Keyboard) || Input.GetKeyUp(joy4);
        public static bool ShootDown2 => Input.GetKeyDown(ability4Keyboard) || Input.GetKeyDown(joy5);
        public static bool ShootUp2 => Input.GetKeyUp(ability4Keyboard) || Input.GetKeyUp(joy5);

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _velocityHorizontalSpeed = _defaultHorizontalVelocity;
            _rigidbody.velocity = transform.forward * _defaultHorizontalVelocity;
            for (int i = 0; i < _maxFramesPerAverage; i++)
                _lastInputs.Add(0);
        }

        private void FixedUpdate()
        {   
            RefreshSettingsChanges();
            UpdatePedalForce();
            UpdateSteering();
            UpdateForwardVelocity();
        }

        private void RefreshSettingsChanges()
        {
            if (VerticalForceSlider != null)
            {
                Vector3 sliderVector = new Vector3(0, VerticalForceSlider.value, 0);
                _pedalForce = sliderVector;

                VerticalForceSlider.transform.GetChild(0).GetComponent<Text>().text = VerticalForceSlider.value.ToString("0.00");
            }

            if (HorizontalMaxVelocitySlider != null)
            {
                _maxHorizontalVelocity = HorizontalMaxVelocitySlider.value;

                HorizontalMaxVelocitySlider.transform.GetChild(0).GetComponent<Text>().text = HorizontalMaxVelocitySlider.value.ToString("0.00");
            }

            if (MaxAverageFramesSlider != null)
            {
                _maxFramesPerAverage = ((int)MaxAverageFramesSlider.value);

                if (_lastInputs.Count < _maxFramesPerAverage)
                {
                    for (int i = 0; i < _maxFramesPerAverage - _lastInputs.Count; i++)
                        _lastInputs.Add(_lastInputs.Last());

                    _currentFrame = (_currentFrame + 1) % _maxFramesPerAverage;
                }

                else if (_lastInputs.Count > _maxFramesPerAverage)
                {
                    _lastInputs.RemoveRange(0, _lastInputs.Count - _maxFramesPerAverage);
                    _currentFrame = (_currentFrame + 1) % _maxFramesPerAverage;
                }
                MaxAverageFramesSlider.transform.GetChild(0).GetComponent<Text>().text = MaxAverageFramesSlider.value.ToString();
            }
        }

        private void UpdatePedalForce()
        {
            float pedalPower = Input.GetAxis(PEDAL_AXIS);

            int averageDivider = Mathf.Min(_currentFrame, _maxFramesPerAverage);
            _lastInputs = _lastInputs.Select(e => {return e + pedalPower;}).ToList();
            float averagePedalPower = _lastInputs[_currentFrame] / averageDivider;
            _currentFrame = (_currentFrame + 1) % _maxFramesPerAverage;

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
            // float steer = Input.GetAxis(HORIZONTAL_STEER);
            // transform.Rotate(_steeringSpeed * steer);
            
            float steering = (Input.GetAxis("SteerLeft") - Input.GetAxis("SteerRight"));

#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
            steering *= 0.5f;
#endif

            if (SteerLeft)
                steering += 1f;
            if (SteerRight)
                steering -= 1f;
            
            transform.Rotate(_steeringSpeed * steering);
        }
        
        private void UpdateForwardVelocity()
        {
            float verticalVelocity = _rigidbody.velocity.y;
            float targetSpeed;
            
            if (Accelerate)
                targetSpeed = _maxHorizontalVelocity;
            else if (Breake)
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
        public void QuitGame()
            => Application.Quit();
    }
}