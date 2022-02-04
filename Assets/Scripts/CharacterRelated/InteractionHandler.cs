using UnityEngine;
using UnityEngine.AI;

namespace Khynan_Coding
{
    [RequireComponent(typeof(CharacterController))]
    public class InteractionHandler : MonoBehaviour
    {
        public delegate void InteractionEventHandler(float currentInteractionTimer, float interactionDuration, string interactionName);
        public event InteractionEventHandler OnInteraction;

        public delegate void EndOfInteractionEventHandler();
        public event EndOfInteractionEventHandler OnInteractionEnd;

        [Header("DETECTION PARAMETERS")]
        public LayerMask InteractingLayer;
        float agentStoppingDistanceAtStart;

        private Transform targetHit = null;
        private Transform currentTarget = null;
        public bool isInteracting = false; // Debug

        public bool HasATarget => CurrentTarget != null;

        #region Components
        NavMeshAgent NavMeshAgent => GetComponent<NavMeshAgent>();
        private CharacterController Controller => GetComponent<CharacterController>();
        public Transform TargetHit { get => targetHit; private set => targetHit = value; }
        public Transform CurrentTarget { get => currentTarget; private set => currentTarget = value; }
        #endregion

        private void Start() => agentStoppingDistanceAtStart = NavMeshAgent.stoppingDistance;

        void Update()
        {
            if (!GameManager.Instance.PlayerCanUseActions())
            {
                return;
            }

            if (Helper.IsRightClickPressed()) { ShootRaycastEntityDetection(); }

            MoveToTarget(CurrentTarget);
        }

        void ShootRaycastEntityDetection()
        {
            if (Physics.Raycast(Helper.GetMainCamera().ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, InteractingLayer))
            {
                AssignTarget(hit.transform);
                return;
            }

            //If the player hit the ground - for the moment it reset the interaction
            ResetInteraction();
            Controller.SwitchState(Controller.IdleState);
        }

        private void AssignTarget(Transform hitTransform)
        {
            InteractiveElement interactiveElement = hitTransform.GetComponent<InteractiveElement>();

            if (!interactiveElement) { return; }

            //If last target wasn't known last = current or LastTarget was known but it is not the same as the target hit
            if (!CurrentTarget || CurrentTarget != TargetHit) 
            {
                TargetHit = hitTransform;
                ResetInteraction();

                Controller.SwitchState(Controller.IdleState);

                CurrentTarget = TargetHit;
                TargetHit = null;
                
                Helper.SetAgentStoppingDistance(NavMeshAgent, CurrentTarget.GetComponent<CollectableResource>().MinimumDistanceToInteract);
            }
        }

        private void MoveToTarget(Transform target)
        {
            if (!HasATarget || isInteracting) { return; }

            Controller.currentMovementSpeed = Mathf.Lerp(
                Controller.currentMovementSpeed, 
                Controller.MaxMovementSpeed, 
                Time.deltaTime * Controller.movementSpeedResetMultiplier);

            Controller.SetCharacterSpeed(Controller.currentMovementSpeed, Controller.currentMovementSpeed);

            if (TargetHasBeenReached(target))
            {
                Controller.SetCharacterSpeed(Controller.currentMovementSpeed, Controller.MaxMovementSpeedDividedByX);

                Helper.SetAgentStoppingDistance(NavMeshAgent, agentStoppingDistanceAtStart);

                InteractiveElement interactiveElement = target.GetComponent<InteractiveElement>();
                interactiveElement.StartInteraction(transform);

                Controller.SwitchState(Controller.InteractionState);

                OnInteraction?.Invoke(
                    interactiveElement.CollectionDuration - interactiveElement.CurrentCollectionTimer, 
                    interactiveElement.CollectionDuration, 
                    interactiveElement.InteractionName);

                Debug.Log("Target has been reached, the interaction is starting");

                return;
            }

            Helper.SetAgentDestination(NavMeshAgent, target.position);
        }

        public void ResetInteraction(bool interactionIsComplete = false)
        {
            if (CurrentTarget)
            {
                InteractiveElement interactiveEntity = CurrentTarget.GetComponent<InteractiveElement>();
                interactiveEntity.ExitInteraction();
            }

            CurrentTarget = null;

            OnInteractionEnd?.Invoke();

            if (interactionIsComplete)
            {
                Controller.SwitchState(Controller.IdleState);
            }
        }

        private bool TargetHasBeenReached(Transform target)
        {
            return DistanceFromTarget(target) <= NavMeshAgent.stoppingDistance && (!NavMeshAgent.hasPath || NavMeshAgent.velocity.sqrMagnitude == 0f);
        }

        private float DistanceFromTarget(Transform target)
        {
            return Vector3.Distance(transform.position, target.position);
        }
    }
}