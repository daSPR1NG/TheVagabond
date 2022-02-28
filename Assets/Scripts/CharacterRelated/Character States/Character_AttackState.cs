namespace Khynan_Coding
{
    public class Character_AttackState : CharacterState
    {
        CharacterController _controller;
        CharacterCombatSystem _characterCombatSystem;

        public override void Init(StateManager stateManager)
        {
            _controller = stateManager.CharacterController;
            _characterCombatSystem = stateManager.GetComponent<CharacterCombatSystem>();
        }

        public override void EnterState(StateManager stateManager)
        {
            Init(stateManager);

            _characterCombatSystem.IsAttacking = true;

            AnimatorHelper.PlayThisAnimationOnThisLayer(
                _controller.Animator, 2, 1f, "IsAttacking", _characterCombatSystem.IsAttacking);

            Helper.DebugMessage("Entering <ATTACK> state", stateManager.transform);
        }

        public override void ExitState(StateManager stateManager)
        {
            _characterCombatSystem.IsAttacking = false;

            AnimatorHelper.PlayThisAnimationOnThisLayer(
                _controller.Animator, 2, 0f, "IsAttacking", _characterCombatSystem.IsAttacking);

            Helper.DebugMessage("Exiting <ATTACK> state", stateManager.transform);
        }

        public override void ProcessState(StateManager stateManager)
        {
            base.ProcessState(stateManager);
        }
    }
}