using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    [DisallowMultipleComponent]
    public class ResourcesManager : MonoBehaviour
    {
        public delegate void ResourceEarnsHandler(EarnData earnData);
        public event ResourceEarnsHandler OnEarningResources;
        
        public delegate void ResourceLossHandler(float valueEarned);
        public event ResourceLossHandler OnLosingResources;

        [Header("OVERTIME INCOME SETTINGS")]
        public bool usesOvertimeIncome = false;
        public float overtimeIncome = 5f;
        public float overtimeDelay = 1f;

        [Header("RESSOURCES")]
        public List<Resource> characterResources = new();

        #region Singleton - Awake
        public static ResourcesManager Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
        #endregion

        void Start()
        {
            if (usesOvertimeIncome) InvokeRepeating(nameof(IncreaseRessourcesOvertime), 1, overtimeDelay);
        }

        private void Update()
        {
            //Debug
            if (Input.GetKeyDown(KeyCode.M))
            {
                GetThisRessource(ResourceType.Minerals).AddToCurrentValue(600);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                GetThisRessource(ResourceType.Minerals).RemoveToCurrentValue(600);
            }
        }

        public void GiveResourcesToPlayer(Resource resource)
        {
            GetThisRessource(resource.ResourceType).AddToCurrentValue(resource.CurrentValue);

            OnEarningResources?.Invoke(new EarnData(resource.ResourceName, resource.CurrentValue, resource));
        } 
        
        public void TakeResourcesFromPlayer(ResourceType wantedRessourceType, float value)
        {
            GetThisRessource(wantedRessourceType).RemoveToCurrentValue(value);

            OnLosingResources?.Invoke(value);
        }

        public Resource GetThisRessource(ResourceType resourceType)
        {
            if(characterResources.Count == 0 || !DoesThisResourceExists(resourceType))
            {
                Debug.LogError("The list character resources is empty");
                return null;
            }

            for (int i = 0; i < characterResources.Count; i++)
            {
                if (characterResources[i].ResourceType != resourceType)
                {
                    return characterResources[i];
                }
            }

            return null;
        }

        private bool DoesThisResourceExists(ResourceType resourceType)
        {
            if (characterResources.Count == 0)
            {
                Debug.LogError("The list character resources is empty");
                return false;
            }

            for (int i = 0; i < characterResources.Count; i++)
            {
                if (characterResources[i].ResourceType == resourceType)
                {
                    return true;
                }
            }

            Debug.LogError("The stat type (" + resourceType + ") does not exists.");
            return false;
        }

        private void IncreaseRessourcesOvertime()
        {
            for (int i = 0; i < characterResources.Count; i++)
            {
                characterResources[i].AddToCurrentValue(overtimeIncome);
            }
        }

        #region Editor
        private void OnValidate()
        {
            Editor_SetRessourcesDatas();
        }

        private void Editor_SetRessourcesDatas()
        {
            if (characterResources.Count == 0) { return; }

            for (int i = 0; i < characterResources.Count; i++)
            {
                characterResources[i].InitResource();
            }
        }
        #endregion
    }
}