using Khynan_Survival;
using UnityEngine;

public enum CameraLockState { Locked, Unlocked }

public class Player_CameraController : MonoBehaviour
{
    [Header("CAMERA SETTINGS")]
    [SerializeField] private CameraLockState cameraLockState;

    [Space]

    [Header("INPUTS")]
    [SerializeField] private KeyCode changeCameraLockStateInput;
    [SerializeField] private KeyCode cameraFocusOnTargetInput;

    [Space]

    [Header("FOLLOWING SETTINGS")]
    public Transform target;
    public float followingSpeed = 0.5f;
    private Vector3 offsetFromCharacter;
    private Player_MovementController TargetStateManager => target.GetComponent<Player_MovementController>();

    [Space]

    [Header("CAMERA MOVEMENT SETTINGS")]
    [SerializeField] private float screenEdgesThreshold = 35f;
    [SerializeField] private float cameraMovementSpeed = 15f;
    public bool usesScreenEdgesMovement = false;
    public bool usesDirectionalArrowMovement = false;
    private Vector3 cameraPosition;

    public bool CameraIsLocked => CameraLockState == CameraLockState.Locked;
    public bool CameraIsUnlocked => CameraLockState == CameraLockState.Unlocked;

    public CameraLockState CameraLockState { get => cameraLockState; set => cameraLockState = value; }
    public KeyCode ChangeCameraLockStateInput { get => changeCameraLockStateInput; }
    public KeyCode CameraFocusOnTargetInput { get => cameraFocusOnTargetInput; }

    public static Player_CameraController Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            SetCameraTarget();
        }
    }

    void Start()
    {
        offsetFromCharacter = transform.position;
    }

    private void Update()
    {
        if (!GameManager.Instance.PlayerCanUseActions())
        {
            return;
        }

        if (UtilityClass.IsKeyPressed(ChangeCameraLockStateInput))
        {
            SetCameraLockStateAtRuntime();
        }

        if (UtilityClass.IsKeyMaintained(CameraFocusOnTargetInput))
        {
            FocusOnCharacter();
        }
        else if (UtilityClass.IsKeyUnpressed(CameraFocusOnTargetInput))
        {
            StopFocusOnCharacter();
        }
    }

    private void FixedUpdate()
    {
        if (CameraIsLocked && TargetStateManager is not null)
        {
            FollowCharacter(target);
        }

        if (usesDirectionalArrowMovement && CameraIsUnlocked) MoveCameraWithDirectionalArrows();
        if (usesScreenEdgesMovement && CameraIsUnlocked) MoveCameraOnHittingScreenEdges();
    }

    public void FollowCharacter(Transform targetToFollow)
    {
        //Follow the character, its position updates after a little delay, or not.
        Vector3 desiredPos = targetToFollow.position + offsetFromCharacter;

        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, followingSpeed * Time.fixedDeltaTime);
        transform.position = smoothedPos;
    }

    public void SetCameraLockStateAtRuntime()
    {
        switch (CameraLockState)
        {
            case CameraLockState.Locked:
                CameraLockState = CameraLockState.Unlocked;
                break;
            case CameraLockState.Unlocked:
                CameraLockState = CameraLockState.Locked;
                break;
        }
    }

    #region Camera Focus
    private void FocusOnCharacter()
    {
        if (CameraLockState != CameraLockState.Locked) CameraLockState = CameraLockState.Locked;
    }

    private void StopFocusOnCharacter()
    {
        if (CameraLockState != CameraLockState.Unlocked) CameraLockState = CameraLockState.Unlocked;
    }
    #endregion

    #region Camera movements
    void MoveCameraOnHittingScreenEdges()
    {
        cameraPosition = Vector3.zero;

        // move on +X axis
        if (Input.mousePosition.x >= Screen.width - screenEdgesThreshold)
        {
            cameraPosition.x += cameraMovementSpeed * Time.fixedDeltaTime;
        }
        // move on -X axis
        if (Input.mousePosition.x <= screenEdgesThreshold)
        {
            cameraPosition.x -= cameraMovementSpeed * Time.fixedDeltaTime;
        }
        // move on +Z axis
        if (Input.mousePosition.y >= Screen.height - screenEdgesThreshold)
        {
            cameraPosition.z += cameraMovementSpeed * Time.fixedDeltaTime;
        }
        // move on -Z axis
        if (Input.mousePosition.y <= screenEdgesThreshold)
        {
            cameraPosition.z -= cameraMovementSpeed * Time.fixedDeltaTime;
        }

        SetCameraPosition(cameraPosition);
    }

    void MoveCameraWithDirectionalArrows()
    {
        cameraPosition = Vector3.zero;

        // move on +X axis
        if (UtilityClass.IsKeyMaintained(KeyCode.RightArrow))
        {
            cameraPosition.x += cameraMovementSpeed * Time.fixedDeltaTime;
        }
        // move on -X axis
        if (UtilityClass.IsKeyMaintained(KeyCode.LeftArrow))
        {
            cameraPosition.x -= cameraMovementSpeed * Time.fixedDeltaTime;
        }
        // move on +Z axis
        if (UtilityClass.IsKeyMaintained(KeyCode.UpArrow))
        {
            cameraPosition.z += cameraMovementSpeed * Time.fixedDeltaTime;
        }
        // move on -Z axis
        if (UtilityClass.IsKeyMaintained(KeyCode.DownArrow))
        {
            cameraPosition.z -= cameraMovementSpeed * Time.fixedDeltaTime;
        }

        SetCameraPosition(cameraPosition);
    }

    void SetCameraPosition(Vector3 newPos)
    {
        transform.position += cameraMovementSpeed * Time.fixedDeltaTime * (UtilityClass.GetMainCameraForwardDirection(0) * newPos.z + UtilityClass.GetMainCameraRightDirection(0) * newPos.x).normalized;
    }
    #endregion

    private void SetCameraTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
}