using System.Collections.Generic;
using UnityEngine;

namespace Khynan_Coding
{
    public enum NPCType
    {
        Unassigned, Quest, Vendor,
    }

    public class InteractiveNPC : InteractiveElement
    {
        [Header("NPC SETTINGS")]
        [SerializeField] private NPCType NPCType = NPCType.Unassigned;
        [SerializeField] private GameObject UIWindow;

        private void Awake()
        {
            Helper.DeactivateUIWindow(UIWindow);
        }

        public override void StartInteraction(Transform interactionActor)
        {
            base.StartInteraction(interactionActor);
            Debug.Log("Interaction with NPC");

            DisplayNPCUIWindow(NPCType);
        }

        public override void ExitInteraction()
        {
            base.ExitInteraction();
            HideNPCUIWindow(NPCType);
        }

        private void DisplayNPCUIWindow(NPCType type)
        {
            if (!UIWindow) 
            {
                Debug.LogError("UI window is missing.");
                return; 
            }

            Helper.ActivateUIWindow(UIWindow);
        }

        private void HideNPCUIWindow(NPCType type)
        {
            if (!UIWindow)
            {
                Debug.LogError("UI window is missing.");
                return;
            }

            Helper.DeactivateUIWindow(UIWindow);
        }
    }
}