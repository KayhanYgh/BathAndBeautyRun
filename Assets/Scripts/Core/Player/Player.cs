using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance;

    private void OnEnable()
    {
        Instance = this;
    }

    public bool stop;
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

    [TabGroup("Stages")] public Animator[] characterAnimators;
    [TabGroup("Stages")] public GameObject[] stage0;
    [TabGroup("Stages")] public GameObject[] stage1;
    [TabGroup("Stages")] public GameObject[] stage2;
    [TabGroup("Stages")] public GameObject[] stage3;
    [TabGroup("Stages")] public GameObject[] stage4;
    [TabGroup("Stages")] public UnityEvent onStageChanged;
    [TabGroup("Stages")] public UnityEvent onStageDecreased;


    private static readonly int CharacterRotation = Animator.StringToHash("Rotation");


    private void Start()
    {
        _rotationAnimator = GetComponent<Animator>();

        yRotation = transform.eulerAngles.y;
    }

    private void Update()
    {
        if (dirtyLevel <= 0)
        {
            Score.ScoreManager.Instance.Lose("");
        }
    }

    private void FixedUpdate()
    {
        if (Score.ScoreManager.Instance.Won)
        {
            PlayMotion(PlayerStatus.Dance);
            return;
        }

        if (Score.ScoreManager.Instance.Lost)
        {
            PlayMotion(PlayerStatus.Sad);
            return;
        }

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
        if (stop) return;

        if (!Score.ScoreManager.Instance.isStarted || Score.ScoreManager.Instance.Finished || Score.ScoreManager.Instance.Won || Score.ScoreManager.Instance.Lost) return;


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

            PlayMotion(PlayerStatus.Move);
        }

        PlayMotion(PlayerStatus.Move);
        transform.position += Time.deltaTime * transform.forward * movementSpeed;
        //else
        //{
        //    touchMovementDeltaX = 0;
        //    PlayMotion(PlayerStatus.Idle);
        //}

        CameraManager();
    }

    private int _stage;
    private int _prevStage = 0;
    public void ManageStage()
    {
        if (dirtyLevel > 50) dirtyLevel = 50;
        if (dirtyLevel < 0) dirtyLevel = 0;

        if (dirtyLevel < 15)
        {
            _stage = 0;
        }
        else if (dirtyLevel >= 15 && dirtyLevel < 30)
        {
            _stage = 1;
        }
        else if (dirtyLevel >= 30 && dirtyLevel < 40)
        {
            _stage = 2;
        }
        else if (dirtyLevel >= 40 && dirtyLevel < 45)
        {
            _stage = 3;
        }
        else if (dirtyLevel >= 45 && dirtyLevel <= 50)
        {
            _stage = 4;
        }

        if (_prevStage != _stage)
        {
            if (_prevStage < _stage)
            {
                onStageChanged?.Invoke();
                _prevStage = _stage;
            }
            else if (_prevStage > _stage)
            {
                _prevStage = _stage;
                onStageDecreased?.Invoke();
                stop = false;
            }
        }

        foreach (var item in stage0)
        {
            item.SetActive(_stage == 0);
        }
        foreach (var item in stage1)
        {
            item.SetActive(_stage == 1);
        }
        foreach (var item in stage2)
        {
            item.SetActive(_stage == 2);
        }
        foreach (var item in stage3)
        {
            item.SetActive(_stage == 3);
        }
        foreach (var item in stage4)
        {
            item.SetActive(_stage == 4);
        }
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



    [TabGroup("Sensors")] private RaycastHit _hitGround;
    [TabGroup("Sensors")] private RaycastHit _hitLeft;
    [TabGroup("Sensors")] private RaycastHit _hitRight;
    [TabGroup("Sensors")] private bool _isGrounded;
    [TabGroup("Sensors")] private bool _rightIsBlocked;
    [TabGroup("Sensors")] private bool _leftIsBlocked;
    [TabGroup("Sensors")] public LayerMask sensorLayerMask;
    [TabGroup("Sensors")] public LayerMask groundSensorLayerMask;
    [TabGroup("Sensors")] public float range;
    [TabGroup("Sensors")] public float groundSensorRange;


    private void SensorManager()
    {
        // Left sensors
        _isGrounded = Physics.Raycast(transform.position, -transform.up, out _hitGround, groundSensorRange, groundSensorLayerMask);

        if (!_isGrounded)
        {
            PlayMotion(PlayerStatus.Falling);
        }

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

        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position, -transform.up * groundSensorRange);
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


    public static readonly int Status = Animator.StringToHash("Status");


    public void PlayMotion(PlayerStatus status)
    {
        if (_isGrounded)
        {
            switch (status)
            {
                case PlayerStatus.Idle:
                    foreach (var item in characterAnimators)
                    {
                        item.SetInteger(Status, 0);
                    }
                    break;
                case PlayerStatus.Move:
                    foreach (var item in characterAnimators)
                    {
                        item.SetInteger(Status, 1);
                    }
                    break;
                case PlayerStatus.Falling:
                    foreach (var item in characterAnimators)
                    {
                        item.SetInteger(Status, 2);
                    }
                    break;
                case PlayerStatus.Dance:
                    foreach (var item in characterAnimators)
                    {
                        item.SetInteger(Status, 3);
                    }
                    break;
                case PlayerStatus.Sad:
                    foreach (var item in characterAnimators)
                    {
                        item.SetInteger(Status, 4);
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            foreach (var item in characterAnimators)
            {
                item.SetInteger(Status, 2);
            }
        }
    }

    public enum PlayerStatus
    {
        Idle,
        Move,
        Falling,
        Dance,
        Sad
    }
}
