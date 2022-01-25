using UnityEngine;
using UnityEngine.AI;

namespace Khynan_Coding
{
    public class InteractionHandler : MonoBehaviour
    {
        public delegate void InteractionEventHandler(float currentInteractionTimer, float interactionDuration, string interactionName);
        public static event InteractionEventHandler OnInteraction;

        public delegate void EndOfInteractionEventHandler();
        public static event EndOfInteractionEventHandler OnEndOfInteraction;

        [Header("DETECTION PARAMETERS")]
        public LayerMask EntityLayer;
        public Transform TargetDetected = null;
        public bool isInteracting = false; // Debug
        public bool HasATarget => TargetDetected != null;
        CollectableRessource collectableRessource;
        float agentStartingStoppingDist;

        #region Components
        NavMeshAgent NavMeshAgent => GetComponent<NavMeshAgent>();
        #endregion

        private void Start()
        {
            agentStartingStoppingDist = NavMeshAgent.stoppingDistance;
        }

        void Update()
        {
            if (!GameManager.Instance.PlayerCanUseActions())
            {
                return;
            }

            if (Helper.IsRightClickPressed())
            {
                TryToDetectAnInteractiveEntity();
            }

            CheckRemainingDistanceWithTarget();
        }

        void TryToDetectAnInteractiveEntity()
        {
            if (Physics.Raycast(Helper.GetMainCamera().ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, EntityLayer))
            {
                bool entityIsInteractive = hit.transform.GetComponent<IInteractive>() is not null;

                if (TargetDetected
                    && entityIsInteractive
                    && hit.transform.gameObject != TargetDetected)
                {
                    ResetInteractionState();
                }

                if (entityIsInteractive)
                {
                    Helper.DebugMessage("Entity detected " + hit.transform.name);

                    TargetDetected = hit.transform;

                    collectableRessource = TargetDetected.GetComponent<CollectableRessource>();
                    collectableRessource.interactingObject = transform;

                    TryToInteract();
                }

                return;
            }
            
            ResetInteractionState();
        }

        void TryToInteract()
        {
            float distanceBetweenObjects = Vector3.Distance(transform.position, TargetDetected.position);

            if (distanceBetweenObjects > collectableRessource.minimumDistanceToInteract)
            {
                agentStartingStoppingDist = NavMeshAgent.stoppingDistance;

                Helper.SetAgentStoppingDistance(NavMeshAgent, collectableRessource.minimumDistanceToInteract * 2);
                Helper.SetAgentDestination(NavMeshAgent, TargetDetected.position);
            }
        }

        void CheckRemainingDistanceWithTarget()
        {
            if (TargetDetected is null || !NavMeshAgent.hasPath
                || TargetDetected is not null && isInteracting) { return; }

            float distanceBetweenObjects = Vector3.Distance(transform.position, TargetDetected.position);

            if (distanceBetweenObjects <= collectableRessource.minimumDistanceToInteract * 2
                && !isInteracting)
            {
                NavMeshAgent.stoppingDistance = agentStartingStoppingDist;

                collectableRessource.Interaction(transform);

                //Call UI Event and set the informations - Display
                OnInteraction?.Invoke(collectableRessource.GetCurrentInterationTimer(), collectableRessource.collectionDuration, collectableRessource.interactionName);

                isInteracting = true;
            }
        }

        public void ResetInteractionState()
        {
            if (TargetDetected is not null
                && collectableRessource is not null)
            {
                collectableRessource.ExitInteraction();
                collectableRessource = null;

                TargetDetected = null;
                isInteracting = false;

                //Call UI Event and set the informations - Hide
                OnEndOfInteraction?.Invoke();
            }
        }
    }
}