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

        [Header("APPEARANCE SETTINGS")]
        public List<ResourceAspect> ressourceAspects;

        private int aspectIndex = 0;
        private float durationTracker = 0f;
        private float timerTargetValue;

        private MeshCollider MeshCollider => GetComponent<MeshCollider>();

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

        protected override void Start()
        {
            base.Start();

            if (ressourceAspects.Count > 0)
            { 
                timerTargetValue = CollectionDuration / ressourceAspects.Count;
                UpdateMeshCollider(ressourceAspects[0]);
            }
        }

        protected override void Update() => base.Update();

        #region Interaction - Start / Exit
        public override void StartInteraction(Transform interactionActor)
        {
            base.StartInteraction(interactionActor);

            if (ressourceType == RessourceType.Unassigned || ressourceAmount == 0)
            {
                Debug.LogError("RessourceType is unassigned or the amount of ressource is equal 0.");
            }

            Debug.Log(timerTargetValue);
            durationTracker = 0f;
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

            UpdateAppearance();
        }

        private void UpdateAppearance()
        {
            if (!AnInteractionIsProcessing || ressourceAspects.Count == 0) { return; }

            durationTracker += Time.deltaTime;

            if (durationTracker >= timerTargetValue)
            {
                durationTracker = 0f;
                aspectIndex++;
            }

            if (aspectIndex >= 1f) 
            { 
                ressourceAspects[aspectIndex].UpdateAppearance(ressourceAspects[index: aspectIndex -1], ressourceAspects[aspectIndex]);
                UpdateMeshCollider(ressourceAspects[aspectIndex]);
            }

            Debug.Log("Change aspect :" + " ASPECT N° " + aspectIndex);
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

        private void UpdateMeshCollider(ResourceAspect aspect)
        {
            if(!aspect.AspectGO.GetComponent<MeshFilter>().sharedMesh) { return; }

            MeshCollider.sharedMesh = aspect.AspectGO.GetComponent<MeshFilter>().sharedMesh;
        }
    }
}