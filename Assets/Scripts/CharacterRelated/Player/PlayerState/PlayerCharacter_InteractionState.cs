namespace Khynan_Coding
{
    public class PlayerCharacter_InteractionState : BasicState
    {
        InteractionHandler interactionHandler;
        PlayerController playerController;

        public override void Init(StateManager stateManager)
        {
            interactionHandler = stateManager.InteractionHandler;
            playerController = (PlayerController)stateManager.GetComponent<CharacterController>();
        }

        public override void EnterState(StateManager stateManager)
        {
            Init(stateManager);

            interactionHandler.isInteracting = true;
            AnimatorHelper.PlayThisAnimationOnThisLayer(playerController.Animator, 1, 1f, "IsInteracting", interactionHandler.isInteracting);

            Helper.DebugMessage("Entering <INTERACTION> state", stateManager.transform);
        }

        public override void ExitState(StateManager stateManager)
        {
            interactionHandler.isInteracting = false;
            AnimatorHelper.PlayThisAnimationOnThisLayer(playerController.Animator, 1, 1f, "IsInteracting", interactionHandler.isInteracting);

            Helper.DebugMessage("Exiting <INTERACTION> state", stateManager.transform);
        }

        public override void ProcessState(StateManager stateManager)
        {
            base.ProcessState(stateManager);
        }
    }
}