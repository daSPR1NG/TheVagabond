using UnityEngine;

namespace Khynan_Coding
{
    public interface IInteractive
    {
        public abstract void Interaction(Transform _interactingObject);
        public abstract void ExitInteraction();

        public static class IInteractiveExtension
        {
            public static void ThrowTryingInteractionMessage(Transform target)
            {
                Helper.DebugMessage("STATUS <TRYING> " + "Interaction with : " + target.name);
            }

            public static void ThrowInteractionMessage(Transform target)
            {
                Helper.DebugMessage("STATUS <BEGIN> " + "Interaction with : " + target.name);
            }

            public static void ThrowInteractionExitMessage()
            {
                Helper.DebugMessage("STATUS <END> " + "Interaction exited");
            }
        }
    }
}