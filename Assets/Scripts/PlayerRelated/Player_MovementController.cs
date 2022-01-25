using UnityEngine;
using UnityEngine.AI;

namespace Khynan_Coding
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
    public class Player_MovementController : StateManager
    {
        [Header("MOVEMENT SETTINGS")]
        public float MovementSpeed = 5f;
        public float RotationSpeed = 30f;
        [HideInInspector] public Vector3 DirectionToMove = Vector3.zero;

        #region Components
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

            //Vertical Axis
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
            //Horizontal Axis
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

            //Need a current position, the actual position of the object,
            //Then you add a direction to move towards to multiplied by a speed represented by the movement speed,
            //this one multiplied by time.-fixed-deltaTime
            Rb.position += MovementSpeed * Time.fixedDeltaTime * (Helper.GetMainCameraForwardDirection(0) * DirectionToMove.z + Helper.GetMainCameraRightDirection(0) * DirectionToMove.x).normalized;

            if (DirectionToMove != Vector3.zero)
            {
                InteractionHandler.ResetInteractionState();

                UpdateTransformRotation();
                CharacterAnimationController.SetAnimationFloatValue(
                    CharacterAnimationController.Animator, "CharacterSpeed", MovementSpeed / 4, 1f);

                Helper.ResetAgentDestination(NavMeshAgent);
                SwitchState(MovingState);
            }
        }

        public void SetCharacterSpeed(float newSpeed)
        {
            MovementSpeed = newSpeed;
            NavMeshAgent.speed = newSpeed;
        }

        private void UpdateTransformRotation()
        {
            if (InteractionHandler.HasATarget) { return; }

                //This operation/conversion helps us rotate the character in the right direction relative to the movement direction and the camera's orientation.
                Quaternion targetRotation = Quaternion.LookRotation(
                (Helper.GetMainCameraForwardDirection(0) * DirectionToMove.z +
                Helper.GetMainCameraRightDirection(0) * DirectionToMove.x).normalized, Vector3.up);

            //This operation/conversion helps us rotate the character in the right direction relative to his movement.
            //Quaternion targetRotation = Quaternion.LookRotation(stateManager.DirectionToMove, Vector3.up.normalized);

            transform.localRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.fixedDeltaTime);
        }
    }
}