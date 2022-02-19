using UnityEngine;

namespace Khynan_Coding
{
    public enum StatType
    {
        Unassigned, Health, MovementSpeed, GatherSpeed,
    }

    [System.Serializable]
    public class Stat
    {
        [Header("SETUP")]
        [SerializeField] private string m_Name = "[TYPE HERE]";
        [SerializeField] private StatType type = StatType.Unassigned;

        [Space][Header("VALUES")]
        [SerializeField] private bool needsToMatchMaxValueAtStart = true;
        [SerializeField] private float maxValue = 0;
        public float currentValue = 0;
        

        [Space][Header("CONDITIONNAL SETTINGS")]
        [Range(0, 100)] [SerializeField] private float criticalThresholdValue = 30;

        public string Name { get => m_Name; private set => m_Name = value; }
        public StatType Type { get => type; }

        #region Public references
        public float CurrentValue { get => currentValue; set => currentValue = Mathf.Clamp(value, 0, maxValue); }
        public float MaxValue { get => maxValue; set => maxValue = value; }
        public float CriticalThresholdValue { get => criticalThresholdValue; }
        #endregion

        public void SetStatName(string stringValue)
        {
            if (m_Name == stringValue) { return; }

            m_Name = stringValue;
        }

        public void SetStatCurrentValue(float value)
        {
            if (CurrentValue == value || !needsToMatchMaxValueAtStart) { return; }

            CurrentValue = value;
        }
    }
}