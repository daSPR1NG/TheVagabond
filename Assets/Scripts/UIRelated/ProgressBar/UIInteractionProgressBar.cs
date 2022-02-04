using UnityEngine;

namespace Khynan_Coding
{
    public class UIInteractionProgressBar : UIProgressBar
    {
        [Header("OBSERVED INTERACTION ACTOR")]
        [SerializeField] private InteractionHandler interactionHandler;

        private void OnEnable()
        {
            interactionHandler.OnInteraction += Init;
            interactionHandler.OnInteractionEnd += HideProgressBar;
        }

        private void OnDisable()
        {
            interactionHandler.OnInteraction -= Init;
            interactionHandler.OnInteractionEnd -= HideProgressBar;
        }

        protected override void Update() => base.Update();
    }
}