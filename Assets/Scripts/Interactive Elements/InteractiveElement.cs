using UnityEngine;

namespace Khynan_Coding
{
    public enum InteractionType
    {
        Unassigned, Harvesting, Logging, Mining, Talking,
    }

    public enum IEType
    {
        Unassigned, Direct, Progressive, WithStats,
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

        [Space][Header("VALUE SETTINGS")]
        [Tooltip("This value defines the number of time the interaction would be executed. " +
            "It multiplies the associated animation duration by this value")]
        [SerializeField] private int interactionRepeatTime = 2;
        [SerializeField] private float minimumDistanceToInteract = 1.25f;
        private float _collectionDuration = 1;
        private float _currentCollectionDuration = 0;

        [Space][Header("EFFECT SETTINGS")]
        [SerializeField] private GameObject interactionCompleteVFX;
        [SerializeField] private AudioClip interactionCompleteSFX;

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
        protected Transform InteractionActor { get => _interactionActor; private set => _interactionActor = value; }
        protected int InteractionRepeatTime { get => interactionRepeatTime; }
        public bool IsItTargetedByPlayer { get => _isItTargetedByPlayer; set => _isItTargetedByPlayer = value; }
        public bool IsInteractive { get => _isInteractive; private set => _isInteractive = value; }
        #endregion

        protected virtual void Start() => Init();

        protected virtual void Update() => InteractionProgression();

        #region Init
        protected virtual void Init()
        {
            //Empty for the moment
        }
        #endregion

        #region Interaction - Start / End
        public virtual void StartInteraction(Transform interactionActor)
        {
            InteractionActor = interactionActor;

            InteractionHandler interactionActorInteractionHandler = InteractionActor.GetComponent<InteractionHandler>();
            interactionActorInteractionHandler.SetCorrectInteractionAnimation(InteractionType);

            if (IEType == IEType.Progressive)
            {
                ResetCollectionDuration();
            }

            if (AnInteractionIsProcessing) { return; }

            AnInteractionIsProcessing = true;
        }

        private void ResetCollectionDuration()
        {
            CollectionDuration = GetCollectionDuration();
            _currentCollectionDuration = 0;
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

            InteractionHandler interactionActorInteractionHandler = InteractionActor.GetComponent<InteractionHandler>();
            interactionActorInteractionHandler.ResetCorrectInteractionAnimation();

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

            _currentCollectionDuration += Time.deltaTime;

            Debug.Log("Interaction Progression.");

            if (_currentCollectionDuration >= CollectionDuration)
            {
                _currentCollectionDuration = CollectionDuration;
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
            interactionHandler.ResetInteraction(true);
        }

        private void InteractionCompletedFeedback()
        {
            Debug.Log("interaction has been completed.");

            //if (interactionCompleteVFX)
            //{
            //    GameObject go = Instantiate(interactionCompleteVFX, transform.position, interactionCompleteVFX.transform.rotation);
            //}

            //if (interactionCompleteSFX)
            //{
            //    //AudioHelper.AudioSourcePlay(audioSource, interactionCompleteSFX);
            //}
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