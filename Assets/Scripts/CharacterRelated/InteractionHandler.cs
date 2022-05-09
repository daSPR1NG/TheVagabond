using System.Collections.Generic;
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
        float _agentStoppingDistanceAtStart;

        private Transform targetHit = null;
        private Transform currentTarget = null;
        private Transform closestTarget = null;
        public bool IsInteracting = false; // Debug
        [SerializeField] private List<InteractionData> interactionDatas = new();

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

        [System.Serializable]
        private class InteractionData
        {
            [field: SerializeField] public Transform IETransform { get; set; }
            [field: SerializeField] public InteractionType InteractiveType { get; set; }
            [field: SerializeField] public InteractiveElement InteractiveElement { get; set; }

            public InteractionData(Transform t, InteractiveElement interactiveElement, InteractionType type)
            {
                this.IETransform = t;
                InteractiveElement = interactiveElement;
                this.InteractiveType = type;
            }
        }

        private void Start() => _agentStoppingDistanceAtStart = NavMeshAgent.stoppingDistance;

        void Update()
        {
            if (!GameManager.Instance.PlayerCanUseActions())
            {
                return;
            }

            if (Helper.IsKeyPressed(InputsManager.Instance.GetInputByName("InteractionMB"))) 
            { 
                ShootRaycastToDetectEntity(); 
            }

            if (Helper.IsKeyPressed(KeyCode.Mouse1) && Helper.IsKeyMaintained(KeyCode.LeftShift))
            {
                ShootRaycastAndTryToAddToQueue();
            }

            TryToInteractWithTheClosestTarget();

            MoveToTarget(CurrentTarget);
        }

        void ShootRaycastToDetectEntity()
        {
            if (!Physics.Raycast(
                Helper.GetMainCamera().ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, InteractingLayer))
            {
                //If the player hit the ground - for the moment it resets the interaction
                ResetInteraction(true);
                Controller.SwitchState(Controller.IdleState);
                Debug.Log("Hit terrain with cursor");
                return;
            }

            AssignTarget(hit.transform);

            InteractiveElement interactiveElement = hit.transform.GetComponent<InteractiveElement>();

            AddInteractionToQueue(new InteractionData(
                    hit.transform, interactiveElement, interactiveElement.InteractionType));
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
                ResetInteraction(true);

                Controller.SwitchState(Controller.IdleState);

                CurrentTarget = TargetHit;
                TargetHit = null;

                Helper.SetAgentStoppingDistance(NavMeshAgent, CurrentTarget.GetComponent<InteractiveElement>().MinimumDistanceToInteract);
                interactiveElement.IsItTargetedByPlayer = true;
            }
        }

        #region Closest Target - Set / Try to interact
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

            ClearInteractionDatas();
            AssignTarget(ClosestTarget);

            Helper.SetAgentStoppingDistance(NavMeshAgent, interactiveElement.MinimumDistanceToInteract);
            interactiveElement.IsItTargetedByPlayer = true;

            Debug.Log("Try to interact by pressing input");
        }
        #endregion

        #region Interaction Datas - Add / Remove / Get / Bool check
        private void ShootRaycastAndTryToAddToQueue()
        {
            if (!Physics.Raycast(Helper.GetMainCamera().ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, InteractingLayer))
            {
                return;
            }

            InteractiveElement interactiveElement = hit.transform.GetComponent<InteractiveElement>();

            AddInteractionToQueue(new InteractionData(
                   hit.transform, interactiveElement, interactiveElement.InteractionType));
        }

        private void AddInteractionToQueue(InteractionData interactionData)
        {
            if (IsThisInteractionAlreadyQueued(interactionData.IETransform)) { return; }

            interactionDatas.Add(interactionData);
            interactionData.InteractiveElement.SelectInteractiveElement();

            if (!CurrentTarget) { AssignTarget(interactionData.IETransform); }
        }

        private void RemoveInteractionFromQueue(InteractionData interactionData)
        {
            if (interactionDatas.Count == 0 || !IsThisInteractionAlreadyQueued(interactionData.IETransform)) { return; }

            interactionData.InteractiveElement.DeselectInteractiveElement();
            interactionDatas.Remove(interactionData);
        }

        private void ClearInteractionDatas()
        {
            if (interactionDatas.Count == 0) { return; }

            for (int i = interactionDatas.Count - 1; i >= 0; i--)
            {
                interactionDatas[i].InteractiveElement.DeselectInteractiveElement();
            }

            interactionDatas.Clear();
        }

        private void TryToPoolNewInteraction()
        {
            if (interactionDatas.Count == 0) { return; }

            AssignTarget(interactionDatas[0].IETransform);
        }

        private InteractionData GetExistingIEData(Transform t)
        {
            if (interactionDatas.Count == 0) { return null; }

            for (int i = interactionDatas.Count - 1; i >= 0; i--)
            {
                if (interactionDatas[i].IETransform == t)
                {
                    return interactionDatas[i];
                }
            }

            return null;
        }

        private bool IsThisInteractionAlreadyQueued(Transform t)
        {
            if (interactionDatas.Count == 0) { return false; }

            for (int i = interactionDatas.Count - 1; i >= 0; i--)
            {
                if (interactionDatas[i].IETransform == t)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Distance with target - Move, target reached check, distance calculation
        private void MoveToTarget(Transform target)
        {
            if (!HasATarget || IsInteracting) { return; }

            Controller.MatchCurrentMSToThisValue(Controller.MaxMovementSpeed, Time.deltaTime);

            InteractiveElement interactiveElement = target.GetComponent<InteractiveElement>();
            SetCorrectInteractionAnimation(interactiveElement.InteractionType);

            if (HasTargetBeenReached(target))
            {
                //Reset Controller movement properties
                Controller.SetCurrentMSValue(Controller.MaxMovementSpeedDividedByX);
                Helper.SetAgentStoppingDistance(NavMeshAgent, _agentStoppingDistanceAtStart);
                Controller.LookAtSomething(target);

                //Start interaction
                interactiveElement.StartInteraction(transform);
                RemoveInteractionFromQueue(GetExistingIEData(interactiveElement.transform));
                Controller.SwitchState(Controller.InteractionState);

                OnInteraction?.Invoke(
                    interactiveElement.InteractionType,
                    interactiveElement.CurrentCollectionDuration, 
                    interactiveElement.CollectionDuration, 
                    interactiveElement.InteractionName,
                    interactiveElement.IEType);

                Debug.Log("Target has been reached, the interaction is starting");

                return;
            }

            Helper.SetAgentDestination(NavMeshAgent, target.position);
            Controller.SwitchState(Controller.MovingState);
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

        public void ResetInteraction(bool clearInteractionDatas)
        {
            Debug.Log("Reset interaction.");

            if (!ClosestTarget) { SetClosestTarget(null); }
            
            if (!CurrentTarget) { return; }

            InteractiveElement interactiveEntity = CurrentTarget.GetComponent<InteractiveElement>();
            interactiveEntity.ExitInteraction();

            CurrentTarget = null;
            OnInteractionEnd?.Invoke();

            Controller.SwitchState(Controller.IdleState);

            if (clearInteractionDatas) 
            {
                ClearInteractionDatas();
                return;
            }

            TryToPoolNewInteraction();
        }

        #region Interaction Animation - Set
        private void SetCorrectInteractionAnimation(InteractionType interactionType)
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
        #endregion
    }
}