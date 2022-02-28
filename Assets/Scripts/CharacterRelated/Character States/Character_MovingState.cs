namespace Khynan_Coding
{
    public class Character_MovingState : CharacterState
    {
        CharacterController _controller;

        public override void Init(StateManager stateManager)
        {
            _controller = stateManager.CharacterController;
        }

        public override void EnterState(StateManager stateManager)
        {
            Init(stateManager);

            stateManager.CharacterIsMoving = true;
            AnimatorHelper.SetAnimatorBooleanParameter(_controller.Animator, "IsMoving", stateManager.CharacterIsMoving);
            
            Helper.DebugMessage("Entering <MOVING> state", stateManager.transform); 
        }

        public override void ExitState(StateManager stateManager)
        {
            stateManager.CharacterIsMoving = false;
            AnimatorHelper.SetAnimatorBooleanParameter(_controller.Animator, "IsMoving", stateManager.CharacterIsMoving);

            Helper.DebugMessage("Exiting <MOVING> state", stateManager.transform);
        }

        public override void ProcessState(StateManager stateManager)
        {
            base.ProcessState(stateManager);

            if (!_controller.IsCharacterMoving)
            {
                Helper.DebugMessage("Destination Reached !");
                stateManager.SwitchState(_controller.IdleState);
                return;
            }

            _controller.UpdateCharacterNavMeshAgentRotation(stateManager.NavMeshAgent, stateManager.transform, _controller.RotationSpeed);

            _controller.SetMovementAnimationValue();
        }
    }
}