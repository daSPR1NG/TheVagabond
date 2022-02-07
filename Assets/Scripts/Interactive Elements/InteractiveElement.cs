using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    //Repenser l'interaction comme un TakeDamage - OnTakingDamage.
    //Ce qui veut dire que lorsque le joueur interragie avec une ressource,
    //il lui inflige des dégâts proportionnels à l'efficatité de ses outils.
    //La progressBar en charge de démontrer l'avancée de cette interaction sera une barre de vie =>
    //Dégâts infligés - vie de la ressource (au début 0 - 150 => 25 - 150 => 50 - 150 => etc.)

    public class InteractiveElement : MonoBehaviour, IInteractive, IDetectable
    {
        [Header("SETUP")]
        public Transform InteractionActor;
        [SerializeField] private InteractionType interactionType = InteractionType.Unassigned;
        [SerializeField] private float collectionDurationOffset = 0.35f;
        [SerializeField] private string interactionName = "[Type HERE]";
        [SerializeField] private float minimumDistanceToInteract = 1.25f;
        private float collectionDuration = 1;

        [Header("OTHER SETTINGS")]
        [SerializeField] private GameObject interactionCompleteVFX;
        [SerializeField] private AudioClip interactionCompleteSFX;

        [Header("APPEARANCE SETTINGS")]
        public List<ResourceAspect> ResourceAspects;
        private int aspectIndex = 0;

        private bool anInteractionIsProcessing = false;

        #region References
        public string InteractionName { get => interactionName; }
        public InteractionType InteractionType { get => interactionType; }
        public float MinimumDistanceToInteract { get => minimumDistanceToInteract; }
        public float CollectionDuration { get => collectionDuration; private set => collectionDuration = value; }
        public bool AnInteractionIsProcessing { get => anInteractionIsProcessing; private set => anInteractionIsProcessing = value; }
        private OutlineModule OutlineComponent => GetComponent<OutlineModule>();
        private MeshCollider MeshCollider => GetComponent<MeshCollider>();
        #endregion

        [System.Serializable]
        public class ResourceAspect
        {
            [SerializeField] private string m_Name;
            [SerializeField] private GameObject aspectGO;

            public GameObject AspectGO { get => aspectGO; }

            #region AspectGO - Display / Hide
            private void DisplayNextAppearance(ResourceAspect aspect)
            {
                aspect.AspectGO.SetActive(true);
            }

            private void HidePreviousAppearance(ResourceAspect aspect)
            {
                aspect.AspectGO.SetActive(false);
            }
            #endregion

            public void UpdateAppearance(ResourceAspect previousAspect, ResourceAspect nextAspect)
            {
                HidePreviousAppearance(previousAspect);
                DisplayNextAppearance(nextAspect);
            }
        }

        protected virtual void Start() => Init();

        protected virtual void Update() => InteractionProgression();

        #region Init
        private void Init()
        {
            if (ResourceAspects.Count > 0)
            {
                UpdateMeshCollider(ResourceAspects[0]);
            }
        }
        #endregion

        #region Interaction - Start / End
        public virtual void StartInteraction(Transform interactionActor)
        {
            InteractionActor = interactionActor;

            InteractionHandler interactionActorInteractionHandler = InteractionActor.GetComponent<InteractionHandler>();
            interactionActorInteractionHandler.SetCorrectInteractionAnimation(InteractionType);

            if (!AnInteractionIsProcessing)
            {
                AnInteractionIsProcessing = true;
            }
        }

        public virtual void ExitInteraction()
        {
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
            //Call Take damage here

            aspectIndex++;
            UpdateAppearance(aspectIndex);
        }

        private void UpdateAppearance(int index)
        {
            ResourceAspects[index].UpdateAppearance(ResourceAspects[index], ResourceAspects[index]);
            UpdateMeshCollider(ResourceAspects[index]);

            Debug.Log("Change aspect :" + " ASPECT N° " + index);
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

        private void UpdateMeshCollider(ResourceAspect aspect)
        {
            if (!aspect.AspectGO.GetComponent<MeshFilter>().sharedMesh) { return; }

            MeshCollider.sharedMesh = aspect.AspectGO.GetComponent<MeshFilter>().sharedMesh;
        }
    }
}