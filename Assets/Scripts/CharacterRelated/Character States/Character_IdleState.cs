namespace Khynan_Coding
{
    public class Character_IdleState : CharacterState
    {
        CharacterController controller;

        public override void Init(StateManager stateManager)
        {
            controller = stateManager.CharacterController;
        }

        public override void EnterState(StateManager stateManager)
        {
            Init(stateManager);
            Helper.ResetAgentDestination(controller.NavMeshAgent);

            Helper.DebugMessage("Entering <IDLE> state", stateManager.transform);
        }

        public override void ExitState(StateManager stateManager)
        {
            controller.ResetTimeSpentInIdleValue();

            Helper.DebugMessage("Exiting <IDLE> state", stateManager.transform);
        }

        public override void ProcessState(StateManager stateManager)
        {
            base.ProcessState(stateManager);

            controller.KeepTrackOfTimeSpentInIdle();
        }
    }
}