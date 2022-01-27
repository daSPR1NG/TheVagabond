namespace Khynan_Coding
{
    public class PlayerCharacter_IdleState : BasicState
    {
        PlayerController playerController;

        public override void Init(StateManager stateManager)
        {
            playerController = (PlayerController)stateManager.GetComponent<CharacterController>();
        }

        public override void EnterState(StateManager stateManager)
        {
            Init(stateManager);
            Helper.ResetAgentDestination(playerController.NavMeshAgent);

            Helper.DebugMessage("Entering <IDLE> state", stateManager.transform);
        }

        public override void ExitState(StateManager stateManager)
        {
            Helper.DebugMessage("Exiting <IDLE> state", stateManager.transform);
        }

        public override void ProcessState(StateManager stateManager)
        {
            base.ProcessState(stateManager);
        }
    }
}