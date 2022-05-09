using UnityEngine;

namespace Khynan_Coding
{
    public enum CameraLockState { Locked, Unlocked }

    public class Player_CameraController : MonoBehaviour
    {
        [Header("CAMERA SETTINGS")]
        [SerializeField] private CameraLockState cameraLockState;

        #region Inputs
        private KeyCode CameraLockStateInput => InputsManager.Instance.GetInputByName("CameraLockState");
        private KeyCode CameraFocusInput => InputsManager.Instance.GetInputByName("CameraFocus");
        #endregion

        [Space][Header("FOLLOWING SETTINGS")]
        public Transform Target;
        public float FollowingSpeed = 0.5f;
        private Vector3 _offsetFromCharacter;
        private PlayerController _TargetStateManager => Target.GetComponent<PlayerController>();

        [Space][Header("CAMERA MOVEMENT SETTINGS")]
        [SerializeField] private float screenEdgesThreshold = 35f;
        [SerializeField] private float cameraMovementSpeed = 15f;
        public bool UsesScreenEdgesMovement = false;
        public bool UsesDirectionalArrowMovement = false;
        private Vector3 _cameraPosition;

        [Space]
        [Header("CAMERA SCROLLING SETTINGS")]
        [SerializeField] private bool isScrollingEnabled = true;
        [SerializeField] private float scrollMinValue = 5f;
        [SerializeField] private float scrollMaxValue = 90f;
        [SerializeField] private float scrollSensitivity = 1.5f;
        [SerializeField] private float scrollStep = 15f;
        private float _cameraScrollValue = 0f;

        public bool CameraIsLocked => CameraLockState == CameraLockState.Locked;
        public bool CameraIsUnlocked => CameraLockState == CameraLockState.Unlocked;

        public CameraLockState CameraLockState { get => cameraLockState; set => cameraLockState = value; }

        #region Singleton
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
        #endregion

        void Start() => Init();

        private void Update()
        {
            if (!GameManager.Instance.PlayerCanUseActions())
            {
                return;
            }

            if (Helper.IsKeyPressed(CameraLockStateInput))
            {
                SetCameraLockStateAtRuntime();
            }

            FocusOnCharacter();
            CameraZoom();
        }

        private void FixedUpdate()
        {
            FollowCharacter(Target);

            if (UsesDirectionalArrowMovement && CameraIsUnlocked) MoveCameraWithDirectionalArrows();
            if (UsesScreenEdgesMovement && CameraIsUnlocked) MoveCameraOnHittingScreenEdges();
        }

        private void Init()
        {
            _cameraScrollValue = scrollMaxValue;
            _offsetFromCharacter = transform.position;
        }

        private Vector3 refPos;
        public void FollowCharacter(Transform targetToFollow)
        {
            if (!CameraIsLocked || !_TargetStateManager) { return; }

            //Follow the character, its position updates after a little delay, or not.
            Vector3 desiredPos = targetToFollow.position + _offsetFromCharacter;

            Vector3 smoothedPos = 
                Vector3.SmoothDamp(transform.position, desiredPos, ref refPos, Time.fixedDeltaTime * FollowingSpeed);
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
            if (Helper.IsKeyMaintained(CameraFocusInput) && CameraLockState != CameraLockState.Locked)
            {
                CameraLockState = CameraLockState.Locked;
                return;
            }

            StopFocusOnCharacter();
        }

        private void StopFocusOnCharacter()
        {
            if (Helper.IsKeyUnpressed(CameraFocusInput) && CameraLockState != CameraLockState.Unlocked)
            {
                CameraLockState = CameraLockState.Unlocked;
            }
        }
        #endregion

        #region Camera movements
        void MoveCameraOnHittingScreenEdges()
        {
            _cameraPosition = Vector3.zero;

            // move on +X axis
            if (Input.mousePosition.x >= Screen.width - screenEdgesThreshold)
            {
                _cameraPosition.x += cameraMovementSpeed * Time.fixedDeltaTime;
            }
            // move on -X axis
            if (Input.mousePosition.x <= screenEdgesThreshold)
            {
                _cameraPosition.x -= cameraMovementSpeed * Time.fixedDeltaTime;
            }
            // move on +Z axis
            if (Input.mousePosition.y >= Screen.height - screenEdgesThreshold)
            {
                _cameraPosition.z += cameraMovementSpeed * Time.fixedDeltaTime;
            }
            // move on -Z axis
            if (Input.mousePosition.y <= screenEdgesThreshold)
            {
                _cameraPosition.z -= cameraMovementSpeed * Time.fixedDeltaTime;
            }

            SetCameraPosition(_cameraPosition);
        }

        void MoveCameraWithDirectionalArrows()
        {
            _cameraPosition = Vector3.zero;

            // move on +X axis
            if (Helper.IsKeyMaintained(KeyCode.RightArrow))
            {
                _cameraPosition.x += cameraMovementSpeed * Time.fixedDeltaTime;
            }
            // move on -X axis
            if (Helper.IsKeyMaintained(KeyCode.LeftArrow))
            {
                _cameraPosition.x -= cameraMovementSpeed * Time.fixedDeltaTime;
            }
            // move on +Z axis
            if (Helper.IsKeyMaintained(KeyCode.UpArrow))
            {
                _cameraPosition.z += cameraMovementSpeed * Time.fixedDeltaTime;
            }
            // move on -Z axis
            if (Helper.IsKeyMaintained(KeyCode.DownArrow))
            {
                _cameraPosition.z -= cameraMovementSpeed * Time.fixedDeltaTime;
            }

            SetCameraPosition(_cameraPosition);
        }

        void SetCameraPosition(Vector3 newPos)
        {
            transform.position += cameraMovementSpeed * Time.fixedDeltaTime * (Helper.GetMainCameraForwardDirection(0) * newPos.z + Helper.GetMainCameraRightDirection(0) * newPos.x).normalized;
        }
        #endregion

        private void SetCameraTarget()
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        float cameraXRotation = 50.25f;
        float cameraYPosition = 25f;

        void CameraZoom()
        {
            if (!isScrollingEnabled && Input.mouseScrollDelta.y == 0) { return; }

            if (/*Input.mouseScrollDelta.y > 0*/ Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                _cameraScrollValue -= scrollStep * Time.deltaTime * scrollSensitivity * 1.5f;
                cameraXRotation -= scrollStep * Time.deltaTime * scrollSensitivity * 2;
                cameraYPosition -= scrollStep * Time.deltaTime * scrollSensitivity;
            }
            if (/*Input.mouseScrollDelta.y < 0*/ Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                _cameraScrollValue += scrollStep * Time.deltaTime * scrollSensitivity * 1.5f;
                cameraXRotation += scrollStep * Time.deltaTime * scrollSensitivity * 2;
                cameraYPosition += scrollStep * Time.deltaTime * scrollSensitivity;
            }

            cameraXRotation = Mathf.Clamp(cameraXRotation, 7.5f, 50.25f);
            transform.eulerAngles = new Vector3(cameraXRotation, transform.eulerAngles.y, 0);

            cameraYPosition = Mathf.Clamp(cameraYPosition, 3.75f, 25);
            transform.position = new Vector3(transform.position.x, cameraYPosition, transform.position.z);

            _cameraScrollValue = Mathf.Clamp(_cameraScrollValue, scrollMinValue, scrollMaxValue);
            transform.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Lens.FieldOfView = _cameraScrollValue;
        }
    }
}