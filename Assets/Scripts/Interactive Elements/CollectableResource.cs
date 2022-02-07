using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    [RequireComponent(typeof(OutlineModule))]
    public class CollectableResource : InteractiveElement
    {
        [Header("RESSOURCE SETTINGS")]
        [SerializeField] private RessourceType ressourceType = RessourceType.Unassigned;
        [SerializeField] private float ressourceAmount = 250f;

        protected override void Update() => base.Update();

        #region Interaction - Start / Exit
        public override void StartInteraction(Transform interactionActor)
        {
            base.StartInteraction(interactionActor);

            if (ressourceType == RessourceType.Unassigned || ressourceAmount == 0)
            {
                Debug.LogError("RessourceType is unassigned or the amount of ressource is equal 0.");
            }
        }

        public override void ExitInteraction()
        {
            base.ExitInteraction();
        }
        #endregion

        #region Interaction - Progression
        protected override void InteractionProgression()
        {
            base.InteractionProgression();
        }
        #endregion

        #region Interaction - Completion
        protected override void OnInteractionCompleted(Transform interactionActor)
        {
            ResourcesManager ressourcesHandlerRef = InteractionActor.GetComponent<ResourcesManager>();
            ressourcesHandlerRef.GetThisRessource(ressourceType).AddToCurrentValue(ressourceAmount);

            Debug.Log("Resources have been given to actor.");

            base.OnInteractionCompleted(interactionActor);
        }
        #endregion
    }
}