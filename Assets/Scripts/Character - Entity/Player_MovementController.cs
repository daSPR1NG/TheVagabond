using UnityEngine;
using UnityEngine.AI;

namespace Khynan_Survival
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    public class Player_MovementController : StateManager
    {
        [Header("MOVEMENT SETTINGS")]
        public float MovementSpeed = 5f;
        public float RotationSpeed = 30f;
        [HideInInspector] public Vector3 DirectionToMove = Vector3.zero;

        #region Components
        [Header("COMPONENTS")]
        public Animator CharacterAnimator;
        public NavMeshAgent NavMeshAgent => GetComponent<NavMeshAgent>();
        public Rigidbody Rb => GetComponent<Rigidbody>();
        #endregion

        private void Start()
        {
            SetCharacterSpeed(10f);
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
            if (UtilityClass.IsKeyPressed(KeyCode.Z) 
                || UtilityClass.IsKeyMaintained(KeyCode.Z))
            {
                DirectionToMove.z = 1;
            }
            if (UtilityClass.IsKeyPressed(KeyCode.S) 
                || UtilityClass.IsKeyMaintained(KeyCode.S))
            {
                DirectionToMove.z = -1;
            }
            //Horizontal Axis
            if (UtilityClass.IsKeyPressed(KeyCode.Q) 
                || UtilityClass.IsKeyMaintained(KeyCode.Q))
            {
                DirectionToMove.x = -1;
            }
            if (UtilityClass.IsKeyPressed(KeyCode.D) 
                || UtilityClass.IsKeyMaintained(KeyCode.D))
            {
                DirectionToMove.x = 1;
            }

            //Need a current position, the actual position of the object,
            //Then you add a direction to move towards to multiplied by a speed represented by the movement speed, this one multiplied by time.-fixed-deltaTime
            Rb.position += MovementSpeed * Time.fixedDeltaTime * (UtilityClass.GetMainCameraForwardDirection(0) * DirectionToMove.z + UtilityClass.GetMainCameraRightDirection(0) * DirectionToMove.x).normalized;

            if (DirectionToMove != Vector3.zero)
            {
                ResetInteractionState();

                UtilityClass.ResetAgentDestination(NavMeshAgent);
                SwitchState(MovingState);
            }
        }

        private void ResetInteractionState()
        {
            InteractionHandler interactionHandler = GetComponent<InteractionHandler>();

            if (interactionHandler.TargetDetected is not null)
            {
                interactionHandler.ResetInteractingState();
            }
        }

        public void SetCharacterSpeed(float newSpeed)
        {
            MovementSpeed = newSpeed;
            NavMeshAgent.speed = newSpeed;
        }
    }
}