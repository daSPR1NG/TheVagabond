using UnityEngine;

namespace Khynan_Coding
{
    public class InteractiveElement : MonoBehaviour, IInteractive, IDetectable
    {
        [Header("SETUP")]
        public Transform InteractionActor;
        [SerializeField] private string interactionName = "[Type HERE]";
        [SerializeField] private float collectionDuration = 1.25f;
        [SerializeField] private float minimumDistanceToInteract = 1.25f;

        [Header("OTHER SETTINGS")]
        [SerializeField] private GameObject interactionCompleteVFX;
        [SerializeField] private AudioClip interactionCompleteSFX;

        private float currentCollectionTimer;

        private bool anInteractionIsProcessing = false;

        #region References
        public string InteractionName { get => interactionName; }
        public float MinimumDistanceToInteract { get => minimumDistanceToInteract; }
        public float CollectionDuration { get => collectionDuration; }
        public float CurrentCollectionTimer { get => currentCollectionTimer; private set => currentCollectionTimer = value; }
        public bool AnInteractionIsProcessing { get => anInteractionIsProcessing; set => anInteractionIsProcessing = value; }
        private OutlineModule OutlineComponent => GetComponent<OutlineModule>();
        #endregion

        protected virtual void Start() => CurrentCollectionTimer = CollectionDuration;

        protected virtual void Update() => InteractionProgression();

        #region Interaction - Start / End
        public virtual void StartInteraction(Transform interactionActor)
        {
            InteractionActor = interactionActor;

            if (!AnInteractionIsProcessing)
            {
                AnInteractionIsProcessing = true;
            }
        }

        public virtual void ExitInteraction()
        {
            InteractionActor = null;
            AnInteractionIsProcessing = false;
        }
        #endregion

        #region Interaction - Progression / complete
        protected virtual void InteractionProgression()
        {
            if (!AnInteractionIsProcessing) { return; }

            CurrentCollectionTimer -= Time.deltaTime;

            if (CurrentCollectionTimer <= 0)
            {
                CurrentCollectionTimer = 0;
                OnInteractionCompleted(InteractionActor);
            }
        }
        #endregion

        #region Interaction - Completion
        protected virtual void OnInteractionCompleted(Transform interactionActor)
        {
            if (!interactionActor) { return; }

            InteractionHandler interactionHandler = interactionActor.GetComponent<InteractionHandler>();
            interactionHandler.ResetInteraction(true);

            InteractionCompletedFeedback();
        }

        private void InteractionCompletedFeedback()
        {
            Debug.Log("interaction has been completed.");

            if (interactionCompleteVFX)
            {
                GameObject go = Instantiate(interactionCompleteVFX, transform.position, interactionCompleteVFX.transform.rotation);
            }

            if (interactionCompleteSFX)
            {
                //AudioHelper.AudioSourcePlay(audioSource, interactionCompleteSFX);
            }
        }
        #endregion

        #region Cursor Detection
        public void OnMouseEnter()
        {
            IDetectable.IDetectableExtension.SetCursorAppearanceOnDetection(
                CursorType.Ressource, OutlineComponent, true, transform.name + " has been detected.");
        }

        public void OnMouseExit()
        {
            IDetectable.IDetectableExtension.SetCursorAppearanceOnDetection(
                CursorType.Default, OutlineComponent, false, transform.name + " is no longer detected.");
        }

        private void OnMouseOver()
        {
            if (OutlineComponent.enabled && (GameManager.Instance.GameIsPaused()))
            {
                OutlineComponent.enabled = false;
            }
        }
        #endregion
    }
}