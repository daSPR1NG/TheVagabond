using UnityEngine;

namespace Khynan_Coding
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : CharacterController
    {
        public PlayerCharacter_IdleState IdleState = new();
        public PlayerCharacter_MovingState MovingState = new();
        public PlayerCharacter_InteractionState InteractionState = new();

        [HideInInspector] public Vector3 DirectionToMove = Vector3.zero;
        Quaternion targetRotation;
        private bool canUpdatePosition = true;

        #region Public references
        public Rigidbody Rb => GetComponent<Rigidbody>();
        #endregion

        private void Start()
        {
            SetCharacterSpeed(MovementSpeed);
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

            #region Vertical Axis - Z, S
            if (Helper.IsKeyPressed(KeyCode.Z) 
                || Helper.IsKeyMaintained(KeyCode.Z))
            {
                DirectionToMove.z = 1;
            }
            if (Helper.IsKeyPressed(KeyCode.S) 
                || Helper.IsKeyMaintained(KeyCode.S))
            {
                DirectionToMove.z = -1;
            }
            #endregion

            #region Horizontal Axis - Q, D
            if (Helper.IsKeyPressed(KeyCode.Q) 
                || Helper.IsKeyMaintained(KeyCode.Q))
            {
                DirectionToMove.x = -1;
            }
            if (Helper.IsKeyPressed(KeyCode.D) 
                || Helper.IsKeyMaintained(KeyCode.D))
            {
                DirectionToMove.x = 1;
            }
            #endregion

            UpdateRigidbodyPosition(Rb, MovementSpeed, 
                (Helper.GetMainCameraForwardDirection(0) * DirectionToMove.z
                + Helper.GetMainCameraRightDirection(0) * DirectionToMove.x).normalized);

            if (DirectionToMove != Vector3.zero) 
            {
                InteractionHandler.ResetInteraction();
                Helper.ResetAgentDestination(NavMeshAgent);

                UpdateTransformRotation();

                if (CanUpdatePosition(transform.localRotation, targetRotation)) { SwitchState(MovingState); }
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

            if (transform.localRotation == targetRotation)
            {
                canUpdatePosition = true;
                Debug.Log("ROTATION VALUE MATCH");
                return;
            }

            canUpdatePosition = false;
            Debug.Log("ROTATION VALUE DON'T MATCH");
        }
    }
}