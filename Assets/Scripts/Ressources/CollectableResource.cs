using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Khynan_Coding
{
    [RequireComponent(typeof(OutlineModule))]
    public class CollectableResource : MonoBehaviour, IInteractive, IDetectable
    {
        [Header("INTERACTION SETTINGS")]
        public Transform interactionActor;
        [SerializeField] private float minimumDistanceToInteract = 1.25f;
        [SerializeField] private string interactionName = "[Type HERE]";
        private bool anInteractionIsProcessing = false; //public to debug

        [Header("RESSOURCE SETTINGS")]
        [SerializeField] private RessourceType ressourceType = RessourceType.Unassigned;
        [SerializeField] private float ressourceAmount = 1000f;
        [SerializeField] private float collectionDuration = 5f;

        private float localTimer;

        [Header("APPEARANCE SETTINGS")]
        public List<Color> appearances;

        #region References
        public string InteractionName { get => interactionName; }
        public float MinimumDistanceToInteract { get => minimumDistanceToInteract; }
        public float CollectionDuration { get => collectionDuration; }
        private OutlineModule OutlineComponent => GetComponent<OutlineModule>();
        #endregion

        private void Update()
        {
            UpdateInteractionProgress();
        }

        public virtual void ExitInteraction()
        {
            interactionActor = null;
            anInteractionIsProcessing = false;
        }

        public virtual void StartInteraction(Transform interactingActor)
        {
            interactionActor = interactingActor;

            if (!anInteractionIsProcessing && ressourceType != RessourceType.Unassigned && ressourceAmount != 0)
            {
                anInteractionIsProcessing = true;
                localTimer = collectionDuration;
            }
        }

        private void UpdateInteractionProgress()
        {
            if (!anInteractionIsProcessing) { return; }

            //for (int i = 0; i < appearances.Count; i++)
            //{
            //    CollectionDuration / appearances.Count;

            //    UpdateAppearance(i);
            //}

            localTimer -= Time.deltaTime;

            if (localTimer <= 0)
            {
                localTimer = 0;
                DeliverResourcesOnInteractionEnd(interactionActor);
            }
        }

        private void UpdateAppearance(int index)
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.color = appearances[index];

            Debug.Log("Change appearance :" + " APPERANCE N° " + index);
        }

        private void DeliverResourcesOnInteractionEnd(Transform interactionActor)
        {
            if (!Helper.IsNull(ref interactionActor, transform))
            {
                RessourcesManager ressourcesHandlerRef = interactionActor.GetComponent<RessourcesManager>();
                ressourcesHandlerRef.GetThisRessource(ressourceType).AddToCurrentValue(ressourceAmount);

                Debug.Log("Ressources have been given to actor.");

                InteractionHandler interactionHandler = interactionActor.GetComponent<InteractionHandler>();
                interactionHandler.ResetInteraction(true);

                DestroyOnInteractionEnd();
            }
        }

        private void DestroyOnInteractionEnd()
        {
            Debug.Log("Destroy, interaction has been completed.");
            //Call Destruction Animation or change the sprite.
            //Play the SFX.
        }

        #region Cursor Detection
        public void OnMouseEnter()
        {
            IDetectable.IDetectableExtension.SetCursorAppearanceOnDetection(CursorType.Ressource, OutlineComponent, true, transform.name + " has been detected.");
        }

        public void OnMouseExit()
        {
            IDetectable.IDetectableExtension.SetCursorAppearanceOnDetection(CursorType.Default, OutlineComponent, false, transform.name + " is no longer detected.");
        }

        private void OnMouseOver()
        {
            if (OutlineComponent.enabled
                && (GameManager.Instance.GameIsPaused()))
            {
                OutlineComponent.enabled = false;
            }
        }
        #endregion
    }
}