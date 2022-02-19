using UnityEngine;

namespace Khynan_Coding
{
    public interface IInteractive
    {
        public abstract void StartInteraction(Transform interactionActor);
        public abstract void ExitInteraction();
    }
}