using UnityEngine;

namespace Khynan_Coding
{
    public class CharacterController : StateManager
    {
        [Header("MOVEMENT SETTINGS")]
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float rotationSpeed = 360f;

        #region Public references
        public float MovementSpeed { get => movementSpeed; private set => movementSpeed = value; }
        public float RotationSpeed { get => rotationSpeed; private set => rotationSpeed = value; }
        public Animator Animator => transform.GetChild(0).GetComponent<Animator>();
        #endregion

        public void SetCharacterSpeed(float newSpeed)
        {
            MovementSpeed = newSpeed;
            NavMeshAgent.speed = newSpeed;
        }

        protected void UpdateRigidbodyPosition(Rigidbody rb, float speed, Vector3 directionToMoveTowards)
        {
            if (Helper.IsNull(ref rb, transform)) { return; }

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
    }
}