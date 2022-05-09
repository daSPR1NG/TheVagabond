using UnityEngine;

namespace Khynan_Coding
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : CharacterController
    {
        Quaternion targetRotation;

        #region Inputs
        private KeyCode MoveForward => InputsManager.Instance.GetInputByName("MoveForward");
        private KeyCode MoveBackward => InputsManager.Instance.GetInputByName("MoveBackward");
        private KeyCode MoveLeft => InputsManager.Instance.GetInputByName("MoveLeft");
        private KeyCode MoveRight => InputsManager.Instance.GetInputByName("MoveRight");
        #endregion

        [Header("INPUT PRESSURE SETTINGS")]
        [SerializeField] private float inputPressureMaxValue = 10f;
        [SerializeField] private float inputPressureMultiplier = 1.5f;
        [SerializeField] private float inputPressureResetMultiplier = 2.5f;
        private float inputPressureValue = 0;

        #region Public references
        public Rigidbody Rb => GetComponent<Rigidbody>();
        #endregion

        protected override void Start()
        {
            base.Start();
            SetDefaultStateAtStart(IdleState);
        }

        void FixedUpdate() => ProcessZQSDMovement();

        private void ProcessZQSDMovement()
        {
            DirectionToMove = Vector3.zero;

            if (!GameManager.Instance.PlayerCanUseActions())
            {
                return;
            }

            UpdateInputPressureValue();

            #region Vertical Axis - Z, S
            if (Helper.IsKeyPressedOrMaintained(MoveForward))
            {
                DirectionToMove.z = 1;
            }
            if (Helper.IsKeyPressedOrMaintained(MoveBackward))
            {
                DirectionToMove.z = -1;
            }
            #endregion

            #region Horizontal Axis - Q, D
            if (Helper.IsKeyPressedOrMaintained(MoveLeft))
            {
                DirectionToMove.x = -1;
            }
            if (Helper.IsKeyPressedOrMaintained(MoveRight))
            {
                DirectionToMove.x = 1;
            }
            #endregion

            MovePlayerCharacter();

            //Debug.Log("Direction To Move == " + DirectionToMove);
        }

        private void MovePlayerCharacter()
        {
            if (DirectionToMove == Vector3.zero) { return; }

            InteractionHandler.ResetInteraction(true);
            Helper.ResetAgentDestination(NavMeshAgent);

            UpdateTransformRotation();

            if (CanUpdatePosition(transform.localRotation, targetRotation))
            {
                UpdateRigidbodyPosition(Rb, MaxMovementSpeed,
                    (Helper.GetMainCameraForwardDirection(0) * DirectionToMove.z).normalized 
                    + (Helper.GetMainCameraRightDirection(0) * DirectionToMove.x).normalized);

                SwitchState(MovingState);
            }
        }

        private void UpdateTransformRotation()
        {
            if (InteractionHandler.HasATarget) { return; }

            //This operation/conversion helps us rotate the character in the right direction relative to the camera.
            targetRotation = Quaternion.LookRotation(
            (Helper.GetMainCameraForwardDirection(0) * DirectionToMove.z +
            Helper.GetMainCameraRightDirection(0) * DirectionToMove.x).normalized, Vector3.up);

            //This operation/conversion helps us rotate the character in the right direction relative to his movement.
            //Quaternion targetRotation = Quaternion.LookRotation(stateManager.DirectionToMove, Vector3.up.normalized);

            transform.localRotation = Quaternion.RotateTowards(
                transform.rotation, targetRotation, 15f * RotationSpeed * Time.fixedDeltaTime);
        }

        private void UpdateInputPressureValue()
        {
            if (InteractionHandler.HasATarget && !CanUpdatePosition(transform.localRotation, targetRotation)) 
            {
                //On simule un appuie d'input
                AnimatorHelper.SetAnimatorFloatParameter(
                    Animator, "InputPressure", inputPressureValue += Time.fixedDeltaTime, Time.fixedDeltaTime * inputPressureMultiplier);
                return; 
            }

            inputPressureValue =
                !AKeyIsPressed() && !AKeyIsMaintained() ? 
                inputPressureValue = Mathf.Lerp(inputPressureValue, 0f, Time.fixedDeltaTime * inputPressureResetMultiplier) : 
                inputPressureValue += Time.fixedDeltaTime * inputPressureMultiplier;

            inputPressureValue = Mathf.Clamp(inputPressureValue, 0, inputPressureMaxValue);

            AnimatorHelper.SetAnimatorFloatParameter(Animator, "InputPressure", inputPressureValue, Time.fixedDeltaTime * inputPressureMultiplier);

            UpdatePlayerCharacterSpeed();
        }

        private void UpdatePlayerCharacterSpeed()
        {
            float currentMovementSpeed;

            currentMovementSpeed =
               !AKeyIsPressed() && !AKeyIsMaintained() ? MaxMovementSpeedDividedByX : MaxMovementSpeedDividedByX + inputPressureValue;

            currentMovementSpeed = Mathf.Clamp(currentMovementSpeed, MaxMovementSpeedDividedByX, MaxMovementSpeed);

            SetCurrentMSValue(currentMovementSpeed);

            AnimatorHelper.SetAnimatorFloatParameter(Animator, "CharacterSpeedMultiplier", Mathf.Clamp(currentMovementSpeed / 2, 0, 1.25f));
        }

        private bool AKeyIsPressed()
        {
            return Helper.IsKeyPressed(KeyCode.Z) || Helper.IsKeyPressed(KeyCode.Q) || Helper.IsKeyPressed(KeyCode.S) || Helper.IsKeyPressed(KeyCode.D);
        }

        private bool AKeyIsMaintained()
        {
            return Helper.IsKeyMaintained(KeyCode.Z) || Helper.IsKeyMaintained(KeyCode.Q) || Helper.IsKeyMaintained(KeyCode.S) || Helper.IsKeyMaintained(KeyCode.D);
        }
    }
}