using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance;

    private void OnEnable()
    {
        Instance = this;
    }

    [TabGroup("Setting")] public float movementSpeed = 6;
    [TabGroup("Setting")] public float touchSensitivity = 0.6f;
    [TabGroup("Setting")] private Touch _touch;
    [TabGroup("Setting")] private Animator _rotationAnimator;
    [TabGroup("Setting")] private float _rotation;
    [TabGroup("Setting")] public float rotationSpeed;
    [TabGroup("Setting")] public float slowerResetInPercentage;

    [TabGroup("Camera")] public Transform cameraTransform;
    [TabGroup("Camera")] public int slowerCameraThanPlayer;

    [TabGroup("Dirty level")] public int dirtyLevel;
    [TabGroup("Dirty level")] public Slider dirtyLevelSlider;


    private static readonly int CharacterRotation = Animator.StringToHash("Rotation");


    private void Start()
    {
        _rotationAnimator = GetComponent<Animator>();

        yRotation = transform.eulerAngles.y;
    }



    private void FixedUpdate()
    {
        if (!Score.ScoreManager.Instance.isStarted) return;


        SensorManager();
        Movement();
        CurveManager();
    }


    float touchMovementDeltaX;
    /// <summary>
    /// Handles player movement and controlling
    /// </summary>
    private void Movement()
    {
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0); // Getting touch

            // Check if player is moving his/her finger
            if (_touch.phase == TouchPhase.Moved)
            {
                touchMovementDeltaX = _touch.deltaPosition.x;

                if (touchMovementDeltaX > 0 && _rightIsBlocked || touchMovementDeltaX < 0 && _leftIsBlocked)
                {
                    touchMovementDeltaX = 0;
                }

                transform.position += transform.right * touchSensitivity * touchMovementDeltaX * Time.deltaTime;

                LocalRotationManager(true);
            }
            else
            {
                LocalRotationManager(false);
            }


            transform.position += Time.deltaTime * transform.forward * movementSpeed;


        }
        else
        {
            touchMovementDeltaX = 0;
        }

        CameraManager();
    }



    private void LocalRotationManager(bool moving)
    {
        if (moving)
        {
            if (touchMovementDeltaX > 1f)
            {
                _rotation -= Time.deltaTime * rotationSpeed;
            }
            else if (touchMovementDeltaX < -1f)
            {
                _rotation += Time.deltaTime * rotationSpeed;
            }
        }
        else
        {
            if (_rotation > 1)
            {
                _rotation -= Time.deltaTime * (rotationSpeed - ((rotationSpeed * slowerResetInPercentage) / 100));
            }
            else if (_rotation < -1)
            {
                _rotation += Time.deltaTime * (rotationSpeed - ((rotationSpeed * slowerResetInPercentage) / 100));
            }
            else
            {
                _rotation = 0;
            }
        }


        if (_rotation > 50)
        {
            _rotation = 50;
        }

        if (_rotation < -50)
        {
            _rotation = -50;
        }


        _rotationAnimator.SetFloat(CharacterRotation, -_rotation); // Applying rotation
    }


    /// <summary>
    /// Manages camera movement and rotation
    /// </summary>
    private void CameraManager()
    {
        cameraTransform.position = transform.position;

        cameraTransform.eulerAngles = Vector3.MoveTowards(cameraTransform.eulerAngles, transform.eulerAngles, Time.deltaTime * (curveRotationSpeed - ((curveRotationSpeed * slowerCameraThanPlayer) / 100)));
    }



    [TabGroup("Sensors")] private RaycastHit _hitLeft;
    [TabGroup("Sensors")] private RaycastHit _hitRight;
    [TabGroup("Sensors")] private bool _rightIsBlocked;
    [TabGroup("Sensors")] private bool _leftIsBlocked;
    [TabGroup("Sensors")] public LayerMask sensorLayerMask;
    [TabGroup("Sensors")] public float range;



    private void SensorManager()
    {
        // Left sensors
        _leftIsBlocked = Physics.Raycast(transform.position, -transform.right, out _hitLeft, range, sensorLayerMask);

        // Right sensors
        _rightIsBlocked = Physics.Raycast(transform.position, transform.right, out _hitRight, range, sensorLayerMask);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = _leftIsBlocked ? Color.red : Color.green;
        Gizmos.DrawRay(transform.position, -transform.right * range);

        Gizmos.color = _rightIsBlocked ? Color.red : Color.green;
        Gizmos.DrawRay(transform.position, transform.right * range);
    }



    [TabGroup("Curve way")] public float curveRotationSpeed;
    [TabGroup("Curve way")] public float yRotation;



    private void CurveManager()
    {
        if (!transform.eulerAngles.y.Equals(yRotation))
        {
            transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, new Vector3(0, yRotation, 0), Time.deltaTime * curveRotationSpeed);
        }
    }
}
