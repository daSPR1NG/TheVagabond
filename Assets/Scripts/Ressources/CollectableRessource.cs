using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Khynan_Coding
{
    [RequireComponent(typeof(OutlineModule))]
    public class CollectableRessource : MonoBehaviour, IInteractive, IDetectable
    {
        [Header("INTERACTION SETTINGS")]
        public Transform interactingObject;
        public float minimumDistanceToInteract = 1.25f;
        public string interactionName = "[Type HERE]";
        public bool hasAnInteractionOccurring = false; //public to debug

        [Header("RESSOURCE SETTINGS")]
        public RessourceType ressourceType = RessourceType.Unassigned;
        public float ressourceAmount = 1000f;
        public float collectionDuration = 5f;
        float currentCollectionTimerValue;

        [Header("APPEARANCE SETTINGS")]
        public List<Color> appearances;

        private Coroutine interactionCoroutine;

        #region Components
        private OutlineModule OutlineComponent => GetComponent<OutlineModule>();
        #endregion

        private void Start()
        {
            currentCollectionTimerValue = collectionDuration;
        }

        public virtual void ExitInteraction()
        {
            interactingObject = null;

            hasAnInteractionOccurring = false;

            if (interactionCoroutine is not null)
            {
                StopCoroutine(interactionCoroutine);
            }
        }

        public virtual void Interaction(Transform _interactingObject)
        {
            interactingObject = _interactingObject;
            collectionDuration = currentCollectionTimerValue;

            if (ressourceType != RessourceType.Unassigned && ressourceAmount != 0)
            {
                interactionCoroutine = StartCoroutine(InteractionCoroutine());
            }

            Helper.DebugMessage("PIPOUPIPOU");
        }

        IEnumerator InteractionCoroutine()
        {
            //Change the sprite to a damaged one to see the advancement.
            hasAnInteractionOccurring = true;

            for (int i = 0; i < appearances.Count; i++)
            {
                yield return new WaitForSeconds(collectionDuration / appearances.Count);
                //currentCollectionTimerValue -= (collectionDuration / appearances.Count);

                UpdateAppearance(i);
            }

            currentCollectionTimerValue = 0;

            yield return new WaitForEndOfFrame();

            DeliverRessourcesToTheInteractingActor();
        }

        public float GetCurrentInterationTimer()
        {
            return currentCollectionTimerValue;
        }

        private void UpdateAppearance(int index)
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.color = appearances[index];

            Debug.Log("Change appearance :" + " APPERANCE N° " + index);
        }

        private void DeliverRessourcesToTheInteractingActor()
        {
            if (interactingObject is not null)
            {
                RessourcesManager ressourcesHandlerRef = interactingObject.GetComponent<RessourcesManager>();
                ressourcesHandlerRef.GetThisRessource(ressourceType).AddToCurrentValue(ressourceAmount);

                Debug.Log("Ressources have been given to actor.");

                interactingObject.GetComponent<InteractionHandler>().ResetInteractionState();
                ExitInteraction();

                DestroyOnCollectionCompleted();
            }
        }

        private void DestroyOnCollectionCompleted()
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