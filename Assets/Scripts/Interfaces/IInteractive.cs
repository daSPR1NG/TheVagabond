using UnityEngine;

public interface IInteractive
{
    public abstract void Interaction(Transform _interactingObject);
    public abstract void ExitInteraction();

    public static class IInteractiveExtension
    {
        public static void ThrowTryingInteractionMessage(Transform target)
        {
            UtilityClass.DebugMessage("STATUS <TRYING> " + "Interaction with : " + target.name);
        }

        public static void ThrowInteractionMessage(Transform target)
        {
            UtilityClass.DebugMessage("STATUS <BEGIN> " + "Interaction with : " + target.name);
        }

        public static void ThrowInteractionExitMessage()
        {
            UtilityClass.DebugMessage("STATUS <END> " + "Interaction exited");
        }
    }
}
