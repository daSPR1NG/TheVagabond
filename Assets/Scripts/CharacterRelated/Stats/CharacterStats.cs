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
        public delegate void HealthValueChanged(float currentHealth, float maxHealth);
        public event HealthValueChanged OnDamageTaken;
        public event HealthValueChanged OnHealReceived;

        public delegate void DeathHappened(Transform killer);
        public event DeathHappened OnDeathHappened;

        [SerializeField] private CharacterType characterType = CharacterType.Unassigned;
        [SerializeField] private float healthPercentage = 0;
        [SerializeField] private List<Stat> stats = new();
        private Transform _attackerData;

        #region Public references
        public CharacterType CharacterType { get => characterType; }
        public List<Stat> Stats { get => stats; set => stats = value; }
        public bool CharacterIsDead => GetStatByType(StatType.Health).CurrentValue <= 0;
        public float HealthPercentage { get => healthPercentage; protected set => healthPercentage = value; }
        public Transform AttackerData { get => _attackerData; private set => _attackerData = value; }
        #endregion

        protected virtual void Start()
        {
            InitStatsValue();
        }

        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                TakeDamage(transform, transform, 10);
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                Heal(transform, transform, 10);
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
                Stats[i].MatchCurrentValueWithBaseValue();

                UnityEngine.AI.NavMeshAgent navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

                bool canModifyNavMeshAgentSpeed =
                    navMeshAgent && DoesThisStatTypeExists(StatType.MovementSpeed)
                    && navMeshAgent.speed != GetStatByType(StatType.MovementSpeed).CurrentValue;

                if (canModifyNavMeshAgentSpeed)
                {
                    navMeshAgent.speed = GetStatByType(StatType.MovementSpeed).CurrentValue;
                }

                if (DoesThisStatTypeExists(StatType.Health))
                {
                    CalculateHealthPercentage(GetStatByType(StatType.Health).CurrentValue, GetStatByType(StatType.Health).MaxValue);
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

        #region Life Stat | Damage - Heal
        public virtual void TakeDamage(Transform target, Transform attacker, float damage)
        {
            GetStatByType(StatType.Health).CurrentValue -= damage;
            OnDamageTaken?.Invoke(
                GetStatByType(StatType.Health).CurrentValue, 
                GetStatByType(StatType.Health).MaxValue);

            CalculateHealthPercentage(GetStatByType(StatType.Health).CurrentValue, GetStatByType(StatType.Health).MaxValue);

            AttackerData = attacker;

            if (GetStatByType(StatType.Health).CurrentValue <= 0) { OnDeath(); }

            Debug.Log("Combat Info : " + target.name + " took " + damage + " damage by " + attacker.name + " !");
        }

        public virtual void Heal(Transform target, Transform ally, float healAmount)
        {
            GetStatByType(StatType.Health).CurrentValue += healAmount;

            OnDamageTaken?.Invoke(
                GetStatByType(StatType.Health).CurrentValue,
                GetStatByType(StatType.Health).MaxValue);

            CalculateHealthPercentage(GetStatByType(StatType.Health).CurrentValue, GetStatByType(StatType.Health).MaxValue);

            Debug.Log("Combat Info : " + target.name + " has been healed by " + healAmount + " life points, by " + ally.name + " !");
        }

        protected virtual void CalculateHealthPercentage(float current, float max)
        {
            HealthPercentage = current / max * 100f;
        }
        #endregion

        #region Death methods
        protected virtual bool IsCloseToDie()
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

            OnDeathHappened?.Invoke(AttackerData);
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