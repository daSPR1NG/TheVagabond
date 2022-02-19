namespace Khynan_Coding
{
    public class Character_MovingState : CharacterState
    {
        CharacterController controller;

        public override void Init(StateManager stateManager)
        {
            controller = stateManager.CharacterController;
        }

        public override void EnterState(StateManager stateManager)
        {
            Init(stateManager);

            stateManager.CharacterIsMoving = true;
            AnimatorHelper.SetAnimatorBooleanParameter(controller.Animator, "IsMoving", stateManager.CharacterIsMoving);
            
            Helper.DebugMessage("Entering <MOVING> state", stateManager.transform); 
        }

        public override void ExitState(StateManager stateManager)
        {
            stateManager.CharacterIsMoving = false;
            AnimatorHelper.SetAnimatorBooleanParameter(controller.Animator, "IsMoving", stateManager.CharacterIsMoving);

            Helper.DebugMessage("Exiting <MOVING> state", stateManager.transform);
        }

        public override void ProcessState(StateManager stateManager)
        {
            base.ProcessState(stateManager);

            if (!controller.IsCharacterMoving)
            {
                Helper.DebugMessage("Destination Reached !");
                stateManager.SwitchState(controller.IdleState);
                return;
            }

            controller.UpdateCharacterNavMeshAgentRotation(stateManager.NavMeshAgent, stateManager.transform, controller.RotationSpeed);

            controller.SetMovementAnimationValue();
        }
    }
}