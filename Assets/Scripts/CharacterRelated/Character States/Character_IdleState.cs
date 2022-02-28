namespace Khynan_Coding
{
    public class Character_IdleState : CharacterState
    {
        CharacterController _controller;

        public override void Init(StateManager stateManager)
        {
            _controller = stateManager.CharacterController;
        }

        public override void EnterState(StateManager stateManager)
        {
            Init(stateManager);
            Helper.ResetAgentDestination(_controller.NavMeshAgent);

            Helper.DebugMessage("Entering <IDLE> state", stateManager.transform);
        }

        public override void ExitState(StateManager stateManager)
        {
            _controller.ResetTimeSpentInIdleValue();

            Helper.DebugMessage("Exiting <IDLE> state", stateManager.transform);
        }

        public override void ProcessState(StateManager stateManager)
        {
            base.ProcessState(stateManager);

            _controller.KeepTrackOfTimeSpentInIdle();
        }
    }
}