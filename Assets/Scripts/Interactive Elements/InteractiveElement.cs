using UnityEngine;

namespace Khynan_Coding
{
    public enum InteractionType
    {
        Unassigned, Harvesting, Logging, Mining, Talking,
    }

    public enum IEType
    {
        Unassigned, Direct, Progressive
    }

    [DisallowMultipleComponent]
    public class InteractiveElement : MonoBehaviour, IInteractive, IDetectable
    {
        [Header("SETUP")]
        [SerializeField] private string interactionName = "[Type HERE]";
        [SerializeField] private InteractionType interactionType = InteractionType.Unassigned;
        [SerializeField] private IEType interactiveElementType = IEType.Unassigned;
        private Transform _interactionActor;
        private bool _isInteractive = true;
        private bool _isSelected;

        [Space][Header("VALUE SETTINGS")]
        [Tooltip("This value defines the number of time the interaction would be executed. " +
            "It multiplies the associated animation duration by this value")]
        [SerializeField] private int interactionRepeatTime = 2;
        [SerializeField] private float minimumDistanceToInteract = 1.25f;
        private float _collectionDuration = 1;
        private float _currentCollectionDuration = 0;

        [Space][Header("EFFECT SETTINGS")]
        [SerializeField] private AudioClip interactionCompleteSFX;

        private DetectionZone _detectionZone;

        private bool _anInteractionIsProcessing = false;
        private bool _isItTargetedByPlayer = false;

        #region References
        public string InteractionName { get => interactionName; }
        public InteractionType InteractionType { get => interactionType; }
        public IEType IEType { get => interactiveElementType; }
        public float MinimumDistanceToInteract { get => minimumDistanceToInteract; }
        public float CollectionDuration { get => _collectionDuration; private set => _collectionDuration = value; }
        public bool AnInteractionIsProcessing { get => _anInteractionIsProcessing; private set => _anInteractionIsProcessing = value; }
        public OutlineModule OutlineComponent => transform.GetChild(0).GetComponent<OutlineModule>();
        protected Animator Animator => transform.GetChild(0).GetComponent<Animator>();
        protected Transform InteractionActor { get => _interactionActor; private set => _interactionActor = value; }
        protected int InteractionRepeatTime { get => interactionRepeatTime; }
        public bool IsItTargetedByPlayer { get => _isItTargetedByPlayer; set => _isItTargetedByPlayer = value; }
        public bool IsInteractive { get => _isInteractive; private set => _isInteractive = value; }
        public float CurrentCollectionDuration { get => _currentCollectionDuration; protected set => _currentCollectionDuration = value; }
        #endregion

        protected virtual void Start() => Init();

        protected virtual void Update() => InteractionProgression();

        #region Init
        protected virtual void Init()
        {
            OutlineComponent.enabled = false;
            _detectionZone = Helper.GetComponentInChildren<DetectionZone>(transform);
        }
        #endregion

        #region Interaction - Start / End
        public virtual void StartInteraction(Transform interactionActor)
        {
            InteractionActor = interactionActor;

            if (IEType == IEType.Progressive)
            {
                CollectionDuration = GetCollectionDuration();
            }

            if (AnInteractionIsProcessing) { return; }

            AnInteractionIsProcessing = true;
            _detectionZone.RemoveATransformFromTheList(InteractionActor);
        }

        protected float GetInteractionAnimationLength()
        {
            if (!InteractionActor) { return 0; }

            Animator controllerAnimator = InteractionActor.GetComponent<CharacterController>().Animator;

            return AnimatorHelper.GetAnimationLength(controllerAnimator, 6);
        }

        protected float GetCollectionDuration()
        {
            if (!InteractionActor) { return 0; }

            CharacterController cC = InteractionActor.GetComponent<CharacterController>();

            Debug.Log(GetInteractionAnimationLength() * InteractionRepeatTime * (1 / cC.CharacterStats.GetStatByType(StatType.GatherSpeed).CurrentValue));

            return GetInteractionAnimationLength() * InteractionRepeatTime * (1 / cC.CharacterStats.GetStatByType(StatType.GatherSpeed).CurrentValue);
        }

        public virtual void ExitInteraction()
        {
            IsItTargetedByPlayer = false;

            if (!InteractionActor) { return; }

            InteractionActor = null;
            AnInteractionIsProcessing = false;
        }
        #endregion

        #region Interaction - Progression
        protected virtual void InteractionProgression()
        {
            if (IEType != IEType.Progressive) { return; }

            UpdateInteractionProgressionTimer();
        }

        private void UpdateInteractionProgressionTimer()
        {
            if (!AnInteractionIsProcessing) { return; }

            CurrentCollectionDuration += Time.deltaTime;

            Debug.Log("Interaction Progression.");

            if (CurrentCollectionDuration >= CollectionDuration)
            {
                CurrentCollectionDuration = CollectionDuration;
                OnInteractionCompleted(InteractionActor);

                Debug.Log("Interaction Completed.");
            }
        }
        #endregion

        #region Interaction - Completion
        protected virtual void OnInteractionCompleted(Transform interactionActor)
        {
            if (!interactionActor) { return; }

            InteractionCompletedFeedback();

            InteractionHandler interactionHandler = interactionActor.GetComponent<InteractionHandler>();
            interactionHandler.ResetInteraction(false);
        }

        private void InteractionCompletedFeedback()
        {
            Debug.Log("interaction has been completed.");

            AnimatorHelper.SetAnimatorTriggerParameter(Animator, "OnDeath");

            if (interactionCompleteSFX)
            {
                //AudioHelper.AudioSourcePlay(audioSource, interactionCompleteSFX);
            }
        }
        #endregion

        #region IE - Selection / Deselection
        public void SelectInteractiveElement()
        {
            _isSelected = true;
            OutlineComponent.enabled = true;
        }

        public void DeselectInteractiveElement()
        {
            _isSelected = false;

            OutlineComponent.enabled = false;
        }
        #endregion

        public void SetInteractiveValue(bool value)
        {
            IsInteractive = value;
        }

        #region Cursor Detection
        public void OnMouseEnter()
        {
            if (!IsInteractive) { return; }

            IDetectable.IDetectableExtension.SetCursorAppearanceOnDetection(
                CursorType.Ressource, OutlineComponent, true, transform.name + " has been detected.");
        }

        public void OnMouseExit()
        {
            if (!IsInteractive) { return; }

            IDetectable.IDetectableExtension.SetCursorAppearanceOnDetection(
                CursorType.Default, OutlineComponent, _isSelected, transform.name + " is no longer detected.");
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