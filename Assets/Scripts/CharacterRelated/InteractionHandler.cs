using UnityEngine;
using UnityEngine.AI;

namespace Khynan_Coding
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public class InteractionHandler : MonoBehaviour
    {
        public delegate void InteractionStartEventHandler(
            InteractionType type, float currentDuration, float maxDuration, string actionName, IEType interactiveElementType);
        public event InteractionStartEventHandler OnInteraction;

        public delegate void InteractionEndEventHandler();
        public event InteractionEndEventHandler OnInteractionEnd;

        [Header("DETECTION PARAMETERS")]
        public LayerMask InteractingLayer;
        float agentStoppingDistanceAtStart;

        private Transform targetHit = null;
        private Transform currentTarget = null;
        private Transform closestTarget = null;
        public bool IsInteracting = false; // Debug

        public bool HasATarget => CurrentTarget != null;
        public bool isWithinReachOfInteraction = false;

        #region Inputs
        private KeyCode InteractionInput => InputsManager.Instance.GetInputByName("Interaction");
        #endregion

        #region Components & References
        NavMeshAgent NavMeshAgent => GetComponent<NavMeshAgent>();
        private CharacterController Controller => GetComponent<CharacterController>();
        public Transform TargetHit { get => targetHit; private set => targetHit = value; }
        public Transform CurrentTarget { get => currentTarget; private set => currentTarget = value; }
        public Transform ClosestTarget { get => closestTarget; private set => closestTarget = value; }
        #endregion

        private void Start() => agentStoppingDistanceAtStart = NavMeshAgent.stoppingDistance;

        void Update()
        {
            if (!GameManager.Instance.PlayerCanUseActions())
            {
                return;
            }

            if (Helper.IsKeyPressed(InputsManager.Instance.GetInputByName("InteractionMB"))) 
            { 
                ShootRaycastEntityDetection(); 
            }

            TryToInteractWithTheClosestTarget();

            MoveToTarget(CurrentTarget);
        }

        void ShootRaycastEntityDetection()
        {
            if (!Physics.Raycast(
                Helper.GetMainCamera().ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, InteractingLayer))
            {
                //If the player hit the ground - for the moment it reset the interaction
                ResetInteraction();
                Controller.SwitchState(Controller.IdleState);
                Debug.Log("Hit terrain with cursor");
                return;
            }

            AssignTarget(hit.transform);
        }

        public void SetClosestTarget(Transform other)
        {
            ClosestTarget = other;
        }

        private void TryToInteractWithTheClosestTarget()
        {
            bool cantInteractWithClosestTarget = !Helper.IsKeyPressed(InteractionInput) || !isWithinReachOfInteraction || !ClosestTarget;

            if (cantInteractWithClosestTarget) { return; }

            InteractiveElement interactiveElement = ClosestTarget.GetComponent<InteractiveElement>();

            if (!interactiveElement.IsInteractive) { return; }

            CurrentTarget = ClosestTarget;

            Helper.SetAgentStoppingDistance(NavMeshAgent, interactiveElement.MinimumDistanceToInteract);
            interactiveElement.IsItTargetedByPlayer = true;

            Debug.Log("Try to interact by pressing input");
        }

        private void AssignTarget(Transform hitTransform)
        {
            InteractiveElement interactiveElement = hitTransform.GetComponent<InteractiveElement>();
            bool isInteractiveElementOfTheCorrectType = 
                interactiveElement.IEType == IEType.Progressive || interactiveElement.IEType == IEType.Direct;

            if (!interactiveElement || !interactiveElement.IsInteractive || !isInteractiveElementOfTheCorrectType) { return; }

            //If last target wasn't known last = current or LastTarget was known but it is not the same as the target hit
            if (!CurrentTarget || CurrentTarget != TargetHit)
            {
                TargetHit = hitTransform;
                ResetInteraction();

                Controller.SwitchState(Controller.IdleState);

                CurrentTarget = TargetHit;
                TargetHit = null;
                
                Helper.SetAgentStoppingDistance(NavMeshAgent, CurrentTarget.GetComponent<InteractiveElement>().MinimumDistanceToInteract);
                interactiveElement.IsItTargetedByPlayer = true;
            }
        }

        #region Distance with target - Move, target reached check, distance calculation
        private void MoveToTarget(Transform target)
        {
            if (!HasATarget || IsInteracting) { return; }

            Controller.MatchCurrentMSToThisValue(Controller.MaxMovementSpeed, Time.deltaTime);

            if (HasTargetBeenReached(target))
            {
                //Reset Controller properties
                Controller.SetCurrentMSValue(Controller.MaxMovementSpeedDividedByX);
                Helper.SetAgentStoppingDistance(NavMeshAgent, agentStoppingDistanceAtStart);

                //Start interaction
                InteractiveElement interactiveElement = target.GetComponent<InteractiveElement>();
                interactiveElement.StartInteraction(transform);

                Controller.SwitchState(Controller.InteractionState);

                OnInteraction?.Invoke(
                    interactiveElement.InteractionType,
                    0, 
                    interactiveElement.CollectionDuration, 
                    interactiveElement.InteractionName,
                    interactiveElement.IEType);

                Debug.Log("Target has been reached, the interaction is starting");

                return;
            }

            Helper.SetAgentDestination(NavMeshAgent, target.position);
        }

        private bool HasTargetBeenReached(Transform target)
        {
            //Debug.Log(DistanceFromTarget(target) <= NavMeshAgent.stoppingDistance + NavMeshAgent.radius);
            //Debug.Log(DistanceFromTarget(target));

            return DistanceFromTarget(target) <= NavMeshAgent.stoppingDistance + NavMeshAgent.radius;
        }

        private float DistanceFromTarget(Transform target)
        {
            return Vector3.Distance(transform.position, target.position);
        }
        #endregion

        public void ResetInteraction(bool interactionIsComplete = false)
        {
            Debug.Log("Reset interaction.");

            if (!ClosestTarget) { SetClosestTarget(null); }
            
            if (!CurrentTarget) { return; }

            InteractiveElement interactiveEntity = CurrentTarget.GetComponent<InteractiveElement>();
            interactiveEntity.ExitInteraction();

            CurrentTarget = null;
            OnInteractionEnd?.Invoke();

            //A garder idk
            //if (!interactionIsComplete) { return; }

            Controller.SwitchState(Controller.IdleState);
        }

        #region Interaction Animation - Set / Reset
        public void SetCorrectInteractionAnimation(InteractionType interactionType)
        {
            AnimatorAssistant animatorAssistant = Controller.Animator.GetComponent<AnimatorAssistant>();

            switch (interactionType)
            {
                case InteractionType.Harvesting:
                    animatorAssistant.SetAnimatorRunTimeController(1);
                    break;
                case InteractionType.Logging:
                    animatorAssistant.SetAnimatorRunTimeController(2);
                    break;
                case InteractionType.Mining:
                    animatorAssistant.SetAnimatorRunTimeController(3);
                    break;
                case InteractionType.Talking:
                    Debug.Log("SET TALKING ANIMATION");
                    animatorAssistant.SetAnimatorRunTimeController(4);
                    break;
            }
        }

        public void ResetCorrectInteractionAnimation()
        {
            AnimatorAssistant animatorAssistant = Controller.Animator.GetComponent<AnimatorAssistant>();
            animatorAssistant.SetAnimatorRunTimeController(0);
        }
        #endregion
    }
}