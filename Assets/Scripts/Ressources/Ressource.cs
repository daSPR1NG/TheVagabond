using UnityEngine;

namespace Khynan_Coding
{
    [System.Serializable]
    public class Ressource
    {
        [Header("GENERAL SETTINGS")]
        public string ressourceName;
        public RessourceType ressourceType;

        [Space]

        [Header("VALUES")]
        public float startingValue = 0f;
        private float currentValue;
        public float CurrentValue
        {
            get => currentValue;
            set
            {
                if (value <= 0)
                {
                    currentValue = 0;
                }
                else if (maxValue != 0 && value >= maxValue)
                {
                    currentValue = maxValue;
                }
                else currentValue = value;
            }
        }
        public float maxValue;

        public delegate void RessourceValueHandler(RessourceType ressourceType);
        public event RessourceValueHandler OnRessourceValueChanged;

        public void InitValue(float startingValue)
        {
            this.startingValue = startingValue;

            OnRessourceValueChanged?.Invoke(ressourceType);
        }

        public void AddToCurrentValue(float valueToAdd)
        {
            if (CurrentValue == 0 && maxValue != 0 && CurrentValue >= maxValue) return;

            CurrentValue += valueToAdd;

            OnRessourceValueChanged?.Invoke(ressourceType);
            RessourcesHandler.Instance.TriggerUIFeedbackOnRessourceCollectionOrLoss(ressourceType);
        }

        public void RemoveToCurrentValue(float valueToRemove)
        {
            if (CurrentValue == 0) return;

            CurrentValue -= valueToRemove;

            OnRessourceValueChanged?.Invoke(ressourceType);
            RessourcesHandler.Instance.TriggerUIFeedbackOnRessourceCollectionOrLoss(ressourceType);
        }
    }
}