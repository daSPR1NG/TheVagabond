namespace Khynan_Coding
{
    public class Character_InteractionState : CharacterState
    {
        InteractionHandler interactionHandler;
        CharacterController controller;

        public override void Init(StateManager stateManager)
        {
            interactionHandler = stateManager.InteractionHandler;
            controller = stateManager.CharacterController;
        }

        public override void EnterState(StateManager stateManager)
        {
            Init(stateManager);

            interactionHandler.isInteracting = true;
            AnimatorHelper.PlayThisAnimationOnThisLayer(controller.Animator, 1, 1f, "IsInteracting", interactionHandler.isInteracting);

            Helper.DebugMessage("Entering <INTERACTION> state", stateManager.transform);
        }

        public override void ExitState(StateManager stateManager)
        {
            interactionHandler.isInteracting = false;
            AnimatorHelper.PlayThisAnimationOnThisLayer(controller.Animator, 1, 1f, "IsInteracting", interactionHandler.isInteracting);

            Helper.DebugMessage("Exiting <INTERACTION> state", stateManager.transform);
        }

        public override void ProcessState(StateManager stateManager)
        {
            base.ProcessState(stateManager);
        }
    }
}