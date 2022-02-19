using UnityEngine;

namespace Khynan_Coding
{
    public class UIInteractionProgressBar : UIProgressBar
    {
        [SerializeField] private InteractionHandler listenedInteractionHandler;

        private void OnEnable()
        {
            listenedInteractionHandler.OnInteraction += Init;
            listenedInteractionHandler.OnInteractionEnd += HideProgressBar;
        }

        private void OnDisable()
        {
            listenedInteractionHandler.OnInteraction -= Init;
            listenedInteractionHandler.OnInteractionEnd -= HideProgressBar;
        }

        protected override void Start() => base.Start();

        void Update() => UpdateImageFillAmout(MaxFillValue);
    }
}