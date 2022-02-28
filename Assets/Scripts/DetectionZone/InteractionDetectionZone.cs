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
            if (!InteractiveElement.IsInteractive || InteractiveElement.IEType == IEType.WithStats) { return; }

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

            SetPlayerInteractionDatas(other.transform, transform.parent, true);
        }

        #region Add method - OnTriggerEnter
        protected override void AddFoundTransform(Transform other)
        {
            base.AddFoundTransform(other);

            if (!InteractiveElement.IsInteractive || InteractiveElement.IEType == IEType.WithStats) { return; }

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

            if (!InteractiveElement.IsInteractive || InteractiveElement.IEType == IEType.WithStats) { return; }

            SetPlayerInteractionDatas(other, null, false);

            InteractiveElement.OutlineComponent.enabled = false;
            OnTriggerExit?.Invoke();
        }
        #endregion

        private void SetPlayerInteractionDatas(Transform targetFound, Transform parent, bool value)
        {
            InteractionHandler otherInteractionHandler = targetFound.GetComponent<InteractionHandler>();

            if(!otherInteractionHandler.ClosestTarget || otherInteractionHandler.ClosestTarget && otherInteractionHandler.ClosestTarget != parent)
            {
                otherInteractionHandler.isWithinReachOfInteraction = value;
                otherInteractionHandler.SetClosestTarget(parent);
                Debug.Log("TRIGGER STAY");
            }
        }
    }
}