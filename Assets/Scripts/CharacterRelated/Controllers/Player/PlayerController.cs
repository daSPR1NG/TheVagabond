using UnityEngine;

namespace Khynan_Coding
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : CharacterController
    {
        Quaternion targetRotation;

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
            if (!GameManager.Instance.PlayerCanUseActions())
            {
                return;
            }

            DirectionToMove = Vector3.zero;

            UpdateInputPressureValue();

            #region Vertical Axis - Z, S
            if (Helper.IsKeyPressed(KeyCode.Z) || Helper.IsKeyMaintained(KeyCode.Z))
            {
                DirectionToMove.z = 1;
            }
            if (Helper.IsKeyPressed(KeyCode.S) || Helper.IsKeyMaintained(KeyCode.S))
            {
                DirectionToMove.z = -1;
            }
            #endregion

            #region Horizontal Axis - Q, D
            if (Helper.IsKeyPressed(KeyCode.Q) || Helper.IsKeyMaintained(KeyCode.Q))
            {
                DirectionToMove.x = -1;
            }
            if (Helper.IsKeyPressed(KeyCode.D) || Helper.IsKeyMaintained(KeyCode.D))
            {
                DirectionToMove.x = 1;
            }
            #endregion

            MovePlayerCharacter();
        }

        private void MovePlayerCharacter()
        {
            if (DirectionToMove != Vector3.zero)
            {
                InteractionHandler.ResetInteraction();
                Helper.ResetAgentDestination(NavMeshAgent);

                UpdateTransformRotation();

                if (CanUpdatePosition(transform.localRotation, targetRotation))
                {
                    UpdateRigidbodyPosition(Rb, MaxMovementSpeed,
                    (Helper.GetMainCameraForwardDirection(0) * DirectionToMove.z
                    + Helper.GetMainCameraRightDirection(0) * DirectionToMove.x).normalized);

                    SwitchState(MovingState);
                }
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
            currentMovementSpeed =
               !AKeyIsPressed() && !AKeyIsMaintained() ? MaxMovementSpeedDividedByX : MaxMovementSpeedDividedByX + inputPressureValue;

            currentMovementSpeed = Mathf.Clamp(currentMovementSpeed, MaxMovementSpeedDividedByX, MaxMovementSpeed);

            SetCharacterSpeed(currentMovementSpeed, currentMovementSpeed);

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