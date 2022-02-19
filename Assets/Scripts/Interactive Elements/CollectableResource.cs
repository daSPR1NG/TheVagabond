using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    public class CollectableResource : InteractiveElement, IRespawnable
    {
        [Header("RESOURCE SETTINGS")]
        [SerializeField] private Resource resource;
        [SerializeField] private float respawnCooldown = 5f;
        private float _currentRespawnCooldown;

        [Space][Header("APPEARANCE SETTINGS")]
        public List<ResourceAspect> ResourceAspects;
        private int _aspectIndex = 0;
        private float _currentCollectionCycleDuration = 0;

        #region References
        private MeshCollider MeshCollider => GetComponent<MeshCollider>();
        #endregion

        [System.Serializable]
        public class ResourceAspect
        {
            [SerializeField] private string m_Name;
            [SerializeField] private GameObject aspectGO;

            public GameObject AspectGO { get => aspectGO; }
            public string Name { get => m_Name; set => m_Name = value; }

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

        protected override void Start() => base.Start();

        protected override void Update()
        {
            base.Update();
            ProcessRespawnCooldownDuration();
        }

        protected override void Init()
        {
            base.Init();

            resource.InitResource();

            if (ResourceAspects.Count == 0) { return; }

            SetDefaultAspect();
        }

        #region Interaction - Start / Exit
        public override void StartInteraction(Transform interactionActor)
        {
            base.StartInteraction(interactionActor);
            _currentCollectionCycleDuration = 0;

            if (resource.ResourceType == ResourceType.Unassigned || resource.CurrentValue == 0)
            {
                Debug.LogError("RessourceType is unassigned or the amount of ressource is equal 0.");
            }
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
            UpdateInteractionProgressionCycle();
        }

        private void UpdateInteractionProgressionCycle()
        {
            if (!AnInteractionIsProcessing) { return; }

            _currentCollectionCycleDuration += Time.deltaTime;

            if (!(_currentCollectionCycleDuration >= GetInteractionAnimationLength())) { return; }

            _currentCollectionCycleDuration = 0;
            _aspectIndex++;

            UpdateAspect(_aspectIndex);
        }
        #endregion

        #region Interaction - Completion
        protected override void OnInteractionCompleted(Transform interactionActor)
        {
            //Give resources to player
            ResourcesManager ressourcesHandlerRef = InteractionActor.GetComponent<ResourcesManager>();
            ressourcesHandlerRef.GiveResourcesToPlayer(resource);

            //Remove the given amount of this resource
            resource.RemoveToCurrentValue(resource.CurrentValue);
            SetInteractiveValue(false);

            Debug.Log("Resources have been given to actor.");

            base.OnInteractionCompleted(interactionActor);

            _currentRespawnCooldown = respawnCooldown;
        }
        #endregion

        #region Update Aspect - Visual / Collider
        private void SetDefaultAspect()
        {
            for (int i = 0; i < ResourceAspects.Count; i++)
            {
                if (ResourceAspects[i] == ResourceAspects[0]) 
                {
                    ResourceAspects[i].AspectGO.SetActive(true);
                    UpdateMeshCollider(ResourceAspects[0]);
                    continue; 
                }

                ResourceAspects[i].AspectGO.SetActive(false);
            }
        }

        private void UpdateAspect(int index)
        {
            ResourceAspects[index].UpdateAppearance(ResourceAspects[index -1], ResourceAspects[index]);
            UpdateMeshCollider(ResourceAspects[index]);

            Debug.Log("Change aspect :" + " ASPECT N° " + index);
        }

        private void UpdateMeshCollider(ResourceAspect aspect)
        {
            if (!aspect.AspectGO.GetComponent<MeshFilter>().sharedMesh)
            {
                Debug.LogError("Cannot find the mesh filter you're trying to change.", transform);
                return;
            }

            MeshCollider.sharedMesh = aspect.AspectGO.GetComponent<MeshFilter>().sharedMesh;
        }
        #endregion

        #region Respawn - Respawn behaviour + Cooldown process
        private void ProcessRespawnCooldownDuration()
        {
            if (IsInteractive) { return; }

            _currentRespawnCooldown -= Time.deltaTime;

            if (_currentRespawnCooldown <= 0)
            {
                _currentRespawnCooldown = 0;
                OnRespawn();
            }
        }

        public void OnRespawn()
        {
            //This below may be called directly in an animation instead of being called here
            SetInteractiveValue(true);
            resource.AddToCurrentValue(resource.StartingValue);

            SetDefaultAspect();

            //Add respawn animation
            //+ maybe VFX
        }
        #endregion

        #region Editor - OnValidate
        private void OnValidate()
        {
            if (ResourceAspects.Count == 0) { return; }

            for (int i = 0; i < ResourceAspects.Count; i++)
            {
                if (!ResourceAspects[i].AspectGO) { continue; }

                ResourceAspects[i].Name = ResourceAspects[i].AspectGO.name;
            }
        }
        #endregion
    }
}