using UnityEngine;

namespace Khynan_Coding
{
    public class InteractiveNPC : InteractiveElement
    {
        public override void StartInteraction(Transform interactionActor)
        {
            Debug.Log("Interaction with NPC");
        }

        public override void ExitInteraction()
        {
            base.ExitInteraction();
        }
    }
}