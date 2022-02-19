using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    public enum CharacterType
    {
        Unassigned, Player, Resource
    }

    [DisallowMultipleComponent]
    public class CharacterStats : MonoBehaviour, IDamageable, IHealable
    {
        [SerializeField] private CharacterType characterType = CharacterType.Unassigned;
        [SerializeField] private List<Stat> stats = new();

        #region Public references
        public CharacterType CharacterType { get => characterType; }
        public List<Stat> Stats { get => stats; set => stats = value; }
        public bool CharacterIsDead => GetStatByType(StatType.Health).CurrentValue <= 0;
        #endregion

        protected virtual void Start()
        {
            InitStatsValue();
        }

        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                OnDamageTaken(10);
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                OnHealReceived(10);
            }
        }

        #region Stats methods - Init, Getter, IsNullCheck
        private void InitStatsValue()
        {
            if (Stats.Count == 0) { return; }

            for (int i = 0; i < Stats.Count; i++)
            {
                if (Stats[i].Type == StatType.Unassigned) { continue; }

                Stats[i].SetStatName(Stats[i].Type.ToString());
                Stats[i].SetStatCurrentValue(Stats[i].MaxValue);

                UnityEngine.AI.NavMeshAgent navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

                bool canModifyNavMeshAgentSpeed =
                    navMeshAgent && DoesThisStatTypeExists(StatType.MovementSpeed)
                    && navMeshAgent.speed != GetStatByType(StatType.MovementSpeed).CurrentValue;

                if (canModifyNavMeshAgentSpeed)
                {
                    navMeshAgent.speed = GetStatByType(StatType.MovementSpeed).CurrentValue;
                }
            }
        }

        public Stat GetStatByType(StatType statType)
        {
            if (Stats.Count == 0 || !DoesThisStatTypeExists(statType)) 
            { 
                Debug.LogError("No stat found."); 
                return null; 
            }

            for (int i = 0; i < Stats.Count; i++)
            {
                if (Stats[i].Type == statType)
                {
                    return Stats[i];
                }
            }

            Debug.LogError("The stat you were looking for does not match any existing types.");
            return null;
        }

        public bool DoesThisStatTypeExists(StatType statType)
        {
            if (Stats.Count == 0)
            {
                Debug.LogError("The list stats is empty");
                return false;
            }

            for (int i = 0; i < Stats.Count; i++)
            {
                if (Stats[i].Type == statType)
                {
                    return true;
                }
            }

            Debug.LogError("The stat type (" + statType + ") does not exists.");
            return false;
        }
        #endregion

        #region Damage - Heal
        public virtual void OnDamageTaken(float damageTaken)
        {
            GetStatByType(StatType.Health).CurrentValue -= damageTaken;
        }

        public virtual void OnHealReceived(float healAmount)
        {
            GetStatByType(StatType.Health).CurrentValue += healAmount;
        }
        #endregion

        #region Death methods
        public bool IsCloseToDie()
        {
            if (GetStatByType(StatType.Health).CurrentValue <= GetStatByType(StatType.Health).MaxValue * (GetStatByType(StatType.Health).CriticalThresholdValue / 100))
            {
                return true;
            }

            return false;
        }

        protected virtual void OnDeath()
        {
            //Call all the elements that happens on death
        }
        #endregion

        #region Editor - On Validate
        private void OnValidate()
        {
            InitStatsValue();
        }
        #endregion
    }
}