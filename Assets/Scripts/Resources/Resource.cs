using UnityEngine;

namespace Khynan_Coding
{
    public enum ResourceType
    {
        Unassigned, Log, Stone, Food, Minerals,
    }

    [System.Serializable]
    public class Resource
    {
        [Header("GENERAL SETTINGS")]
        [SerializeField] private string resourceName;
        public ResourceType ResourceType;

        [Space]
        [Header("VALUES")]
        [SerializeField] private float startingValue = 0f;
        [SerializeField] private float maxValue;
        private float currentValue;
        public float CurrentValue
        {
            get => currentValue;
            set { currentValue = maxValue > 0 ? Mathf.Clamp(value, 0, MaxValue) : currentValue = value; }
        }

        #region Public references
        public float StartingValue { get => startingValue; }
        public float MaxValue { get => maxValue; }
        public string ResourceName { get => resourceName; private set => resourceName = value; }
        #endregion

        public delegate void ResourceValueHandler(ResourceType ressourceType);
        public event ResourceValueHandler OnRessourceValueChanged;

        public void InitResource()
        {
            CurrentValue = StartingValue;
            ResourceName = ResourceType.ToString();

            OnRessourceValueChanged?.Invoke(ResourceType);
        }

        public void AddToCurrentValue(float valueToAdd)
        {
            if (CurrentValue == 0 && MaxValue != 0 && CurrentValue >= MaxValue) return;

            CurrentValue += valueToAdd;

            OnRessourceValueChanged?.Invoke(ResourceType);
        }

        public void RemoveToCurrentValue(float valueToRemove)
        {
            if (CurrentValue == 0) return;

            CurrentValue -= valueToRemove;

            OnRessourceValueChanged?.Invoke(ResourceType);
        }
    }
}