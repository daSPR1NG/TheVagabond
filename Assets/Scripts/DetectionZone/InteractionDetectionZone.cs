using UnityEngine;

namespace Khynan_Coding
{
    public class InteractionDetectionZone : DetectionZone
    {
        public delegate void TriggerEventHandler();
        public event TriggerEventHandler OnTriggerEnter;
        public event TriggerEventHandler OnTriggerExit;

        #region Public references
        private InteractiveElement InteractiveElement => transform.parent.GetComponent<InteractiveElement>();
        #endregion

        protected override void Start() => base.Start();

        private void OnTriggerStay(Collider other)
        {
            if (!InteractiveElement.IsInteractive) { return; }

            if (!InteractiveElement.OutlineComponent.enabled && !InteractiveElement.AnInteractionIsProcessing) 
            { 
                InteractiveElement.OutlineComponent.enabled = true;
                OnTriggerEnter?.Invoke();
            }

            if(InteractiveElement.OutlineComponent.enabled && InteractiveElement.AnInteractionIsProcessing)
            {
                InteractiveElement.OutlineComponent.enabled = false;
                OnTriggerExit?.Invoke();
            }
        }

        #region Add method - OnTriggerEnter
        protected override void AddFoundTransform(Transform other)
        {
            base.AddFoundTransform(other);

            if (!InteractiveElement.IsInteractive) { return; }

            SetPlayerInteractionDatas(other, transform.parent, true);

            if (InteractiveElement.IsItTargetedByPlayer)
            {
                OnTriggerExit?.Invoke();
                return;
            }

            if (!InteractiveElement.OutlineComponent.enabled)
            {
                InteractiveElement.OutlineComponent.enabled = true;
            }

            OnTriggerEnter?.Invoke();
        }
        #endregion

        #region Remove methods - OnTriggerExit & Dead Flag
        protected override void RemoveATransformFromTheList(Transform other)
        {
            base.RemoveATransformFromTheList(other);

            if (!InteractiveElement.IsInteractive) { return; }

            SetPlayerInteractionDatas(other, null, false);

            InteractiveElement.OutlineComponent.enabled = false;
            OnTriggerExit?.Invoke();
        }
        #endregion

        private void SetPlayerInteractionDatas(Transform other, Transform closestTransform, bool value)
        {
            InteractionHandler otherInteractionHandler = other.GetComponent<InteractionHandler>();
            otherInteractionHandler.isWithinReachOfInteraction = value;
            otherInteractionHandler.SetClosestTarget(closestTransform);
        }
    }
}