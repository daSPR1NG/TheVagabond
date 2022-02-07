using UnityEngine;

namespace Khynan_Coding
{
    [System.Serializable]
    public class Resource
    {
        [Header("GENERAL SETTINGS")]
        [SerializeField] private string ressourceName;
        public RessourceType RessourceType;

        [Space]

        [Header("VALUES")]
        [SerializeField] private float startingValue = 0f;
        [SerializeField] private float maxValue;
        private float currentValue;
        public float CurrentValue
        {
            get => currentValue;
            set { currentValue = Mathf.Clamp(value, 0, MaxValue); }
        }

        #region Public references
        public float StartingValue { get => startingValue; }
        public float MaxValue { get => maxValue; }
        #endregion

        public delegate void RessourceValueHandler(RessourceType ressourceType);
        public event RessourceValueHandler OnRessourceValueChanged;

        public void InitRessource(float startingValue)
        {
            CurrentValue = startingValue;
            ressourceName = RessourceType.ToString();

            OnRessourceValueChanged?.Invoke(RessourceType);
        }

        public void AddToCurrentValue(float valueToAdd)
        {
            if (CurrentValue == 0 && MaxValue != 0 && CurrentValue >= MaxValue) return;

            CurrentValue += valueToAdd;

            OnRessourceValueChanged?.Invoke(RessourceType);
        }

        public void RemoveToCurrentValue(float valueToRemove)
        {
            if (CurrentValue == 0) return;

            CurrentValue -= valueToRemove;

            OnRessourceValueChanged?.Invoke(RessourceType);
        }
    }
}