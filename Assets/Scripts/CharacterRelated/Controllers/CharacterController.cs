using UnityEngine;

namespace Khynan_Coding
{
    public class CharacterController : StateManager
    {
        [Header("MOVEMENT SETTINGS")]
        [SerializeField] private float maxMovementSpeed = 5f;
        public float currentMovementSpeed = 0;
        public float movementSpeedResetMultiplier = 0;
        [SerializeField] private float rotationSpeed = 360f;

        [HideInInspector] public Vector3 DirectionToMove = Vector3.zero;

        private float timeSpentInIdle = 0f;
        private bool firstIdleActionHasBeenSet = false;

        #region Character states
        public Character_IdleState IdleState = new();
        public Character_MovingState MovingState = new();
        public Character_InteractionState InteractionState = new();
        #endregion

        #region Public references
        public float MaxMovementSpeed { get => maxMovementSpeed; private set => maxMovementSpeed = value; }
        public float MaxMovementSpeedDividedByX => MaxMovementSpeed / 4;
        public float RotationSpeed { get => rotationSpeed; private set => rotationSpeed = value; }
        public bool IsCharacterMoving => DirectionToMove != Vector3.zero || NavMeshAgent.hasPath;
        public Animator Animator => transform.GetChild(0).GetComponent<Animator>();
        #endregion

        protected virtual void Start()
        {
            SetCharacterSpeed(MaxMovementSpeed, MaxMovementSpeed);
            SetCharacterSpeed(currentMovementSpeed, MaxMovementSpeedDividedByX);
        }

        public void SetCharacterSpeed(float speedToUpdate, float newSpeed)
        {
            speedToUpdate = newSpeed;
            NavMeshAgent.speed = newSpeed;
        }

        protected void UpdateRigidbodyPosition(Rigidbody rb, float speed, Vector3 directionToMoveTowards)
        {
            //Algo = Current position += direction to move towards * a speed *time.fixed/delta.Time
            rb.position += speed * Time.fixedDeltaTime * directionToMoveTowards;
        }

        public void UpdateCharacterNavMeshAgentRotation(UnityEngine.AI.NavMeshAgent navMeshAgent, Transform transform, float rotationSpeed)
        {
            if (navMeshAgent.hasPath && navMeshAgent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                Quaternion lookRotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    lookRotation,
                    15f * rotationSpeed * Time.fixedDeltaTime);
            }
        }

        public bool CanUpdatePosition(Quaternion localRotation, Quaternion targetRotation)
        {
            if (localRotation == targetRotation)
            {
                return true;
            }

            return false;
        }

        public void KeepTrackOfTimeSpentInIdle()
        {
            timeSpentInIdle += Time.deltaTime;

            //Au bout de t active l'action 01 de l'idle -> change d'animation
            //t = 60
            if (timeSpentInIdle >= 60 && !firstIdleActionHasBeenSet)
            {
                AnimatorHelper.SetAnimatorIntParameter(Animator, "IdleActionValue", 1);
                firstIdleActionHasBeenSet = true;
            }
            
            //Au bout de t * 2 active l'action 01 de l'idle -> change d'animation
            if (timeSpentInIdle >= (60 * 2))
            {
                AnimatorHelper.SetAnimatorIntParameter(Animator, "IdleActionValue", 2);
            }
        }

        public void ResetTimeSpentInIdleValue()
        {
            timeSpentInIdle = 0f;
            AnimatorHelper.SetAnimatorIntParameter(Animator, "IdleActionValue", 0);
            firstIdleActionHasBeenSet = false;
        }
    }
}