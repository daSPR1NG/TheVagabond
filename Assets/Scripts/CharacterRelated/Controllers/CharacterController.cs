using UnityEngine;

namespace Khynan_Coding
{
    [DisallowMultipleComponent]
    public class CharacterController : StateManager
    {
        [Header("MOVEMENT SETTINGS")]
        public float movementSpeedResetMultiplier = 0;
        [SerializeField] private float rotationSpeed = 360f;

        [HideInInspector] public Vector3 DirectionToMove = Vector3.zero;

        private float timeSpentInIdle = 0f;
        private bool firstIdleActionHasBeenSet = false;

        #region Character states
        public Character_IdleState IdleState = new();
        public Character_MovingState MovingState = new();
        public Character_InteractionState InteractionState = new();
        public Character_AttackState AttackState = new();
        #endregion

        #region Public references
        public float MaxMovementSpeedDividedByX => CharacterStats.GetStatByType(StatType.MovementSpeed).MaxValue / 4;
        public float RotationSpeed { get => rotationSpeed; private set => rotationSpeed = value; }
        public bool IsCharacterMoving => DirectionToMove != Vector3.zero || NavMeshAgent.hasPath;
        public float MaxMovementSpeed => CharacterStats.GetStatByType(StatType.MovementSpeed).MaxValue;
        public CharacterStats CharacterStats => GetComponent<CharacterStats>();
        public Animator Animator => transform.GetChild(0).GetComponent<Animator>();
        #endregion

        protected virtual void Start()
        {
            SetCurrentMSValue(MaxMovementSpeedDividedByX);
        }

        public void SetCurrentMSValue(float value)
        {
            CharacterStats.GetStatByType(StatType.MovementSpeed).CurrentValue = value;
            NavMeshAgent.speed = value;
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

        public void SetMovementAnimationValue()
        {
            AnimatorHelper.SetAnimatorFloatParameter(
                Animator, "CharacterSpeed", CharacterStats.GetStatByType(StatType.MovementSpeed).CurrentValue);
            AnimatorHelper.SetAnimatorFloatParameter(
                Animator, "CharacterSpeedMultiplier", CharacterStats.GetStatByType(StatType.MovementSpeed).CurrentValue / 2.5f);
        }

        public void MatchCurrentMSToThisValue(float valueToMatch, float updateSpeed)
        {
            float currentMovementSpeed = CharacterStats.GetStatByType(StatType.MovementSpeed).CurrentValue;

            currentMovementSpeed = Mathf.Lerp(currentMovementSpeed, valueToMatch, updateSpeed);

            SetCurrentMSValue(currentMovementSpeed);
        }
    }
}