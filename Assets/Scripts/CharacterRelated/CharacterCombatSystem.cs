using UnityEngine;

namespace Khynan_Coding
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterStats), typeof(CharacterController), typeof(CharacterGearInventory))]
    public class CharacterCombatSystem : MonoBehaviour
    {
        [SerializeField] private GameObject attackColliderGO;
        public bool IsAttacking = false;

        public float ResetAttackTimerValue = 3f;
        private float _internalResetAttackTimerValue;
        private int _attackIndex = 0;

        private bool canAttack = true;
        private float _internalAttackCD = 0;

        #region Public References
        private CharacterStats CharacterStats => GetComponent<CharacterStats>();
        private CharacterController CharacterController => GetComponent<CharacterController>();
        private CharacterGearInventory CharacterInventory => GetComponent<CharacterGearInventory>();
        #endregion

        void Start()
        {
            Init();
        }

        void Update()
        {
            if (!GameManager.Instance.PlayerCanUseActions())
            {
                return;
            }

            if ((Helper.IsKeyPressed(InputsManager.Instance.GetInputByName("AttackMB")) 
                || Helper.IsKeyMaintained(InputsManager.Instance.GetInputByName("AttackMB"))) && canAttack)
            {
                Attack();
            }

            ProcessAttackCooldown();
            ProcessResetAttackTimer();
        }

        void Init()
        {
            //Set all datas that need it at the start of the game
            ResetAttackTimerValue = CharacterStats.GetStatByType(StatType.AttackSpeed).CurrentValue + 0.15f;
        }

        protected virtual void Attack()
        {
            Helper.ResetAgentDestination(CharacterController.NavMeshAgent);
            CharacterController.InteractionHandler.ResetInteraction();

            if (CharacterInventory.CurrentEquippedGear)
            {
                SetAnimatorOverrideController(CharacterInventory.CurrentEquippedGear.GearAOC);
            }

            UpdateAttackIndex();
            CharacterController.SwitchState(CharacterController.AttackState);

            _internalResetAttackTimerValue = ResetAttackTimerValue;

            _internalAttackCD = CharacterStats.GetStatByType(StatType.AttackSpeed).CurrentValue;
            canAttack = false;

            attackColliderGO.SetActive(true);

            Debug.Log("ATTACK");
        }

        private void UpdateAttackIndex()
        {
            if (!canAttack) { return; }

            if (_attackIndex >= 3) { _attackIndex = 0; }

            _attackIndex++;

            AnimatorHelper.SetAnimatorIntParameter(CharacterController.Animator, "AttackIndex", _attackIndex);
        }

        private void ProcessAttackCooldown()
        {
            _internalAttackCD -= Time.deltaTime;

            if (_internalAttackCD <= 0)
            {
                _internalAttackCD = 0;
                canAttack = true;
            }
        }

        private void ProcessResetAttackTimer()
        {
            if (!IsAttacking) { return; }

            _internalResetAttackTimerValue -= Time.deltaTime;

            if (_internalResetAttackTimerValue <= 0)
            {
                CharacterController.SwitchState(CharacterController.IdleState);
                
                _internalResetAttackTimerValue = 0;

                _attackIndex = 0;
                AnimatorHelper.SetAnimatorIntParameter(CharacterController.Animator, "AttackIndex", _attackIndex);
            }
        }

        private void SetAnimatorOverrideController(AnimatorOverrideController animatorOverrideController)
        {
            CharacterController.Animator.runtimeAnimatorController = animatorOverrideController;
        }
    }
}