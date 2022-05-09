using UnityEngine;

namespace Khynan_Coding
{
    public class InteractionDetectionZone : DetectionZone
    {
        public delegate void TriggerEventHandler();
        public event TriggerEventHandler OnTriggerEnter;
        public event TriggerEventHandler OnTriggerExit;

        #region Public references
        private InteractiveElement ParentAttachedIE => transform.parent.GetComponent<InteractiveElement>();
        #endregion

        protected override void Start() => base.Start();

        private void OnTriggerStay(Collider other)
        {
            if (!ParentAttachedIE.IsInteractive) { return; }

            bool displayInteractionFeedback = 
                !ParentAttachedIE.OutlineComponent.enabled && !ParentAttachedIE.AnInteractionIsProcessing;

            if (displayInteractionFeedback)
            {
                AddFoundTransform(other.transform);
                return;
            }
        }

        #region Add method - OnTriggerEnter
        protected override void AddFoundTransform(Transform other)
        {
            base.AddFoundTransform(other);

            if (!ParentAttachedIE.IsInteractive || ParentAttachedIE.IsItTargetedByPlayer) { return; }

            SetPlayerInteractionDatas(other, transform.parent, true);

            if (ParentAttachedIE.IsItTargetedByPlayer) { return; }

            if (!ParentAttachedIE.OutlineComponent.enabled)
            {
                ParentAttachedIE.OutlineComponent.enabled = true;
            }

            OnTriggerEnter?.Invoke();
        }
        #endregion

        #region Remove methods - OnTriggerExit & Dead Flag
        public override void RemoveATransformFromTheList(Transform other)
        {
            base.RemoveATransformFromTheList(other);

            if (!ParentAttachedIE.IsInteractive) { return; }

            SetPlayerInteractionDatas(other, null, false);

            ParentAttachedIE.OutlineComponent.enabled = false;
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