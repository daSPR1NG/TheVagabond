using UnityEngine;

namespace Khynan_Coding
{
    public class Character_InteractionState : BasicState
    {
        public override void EnterState(StateManager stateManager)
        {
            //Play interaction animation 
            //Launch interaction function from the interacted object
            Helper.DebugMessage("Entering <INTERACTION> state", stateManager.GetComponent<Transform>());
        }

        public override void ExitState(StateManager stateManager)
        {
            Helper.DebugMessage("Exiting <INTERACTION> state", stateManager.GetComponent<Transform>());
        }

        public override void ProcessState(StateManager stateManager)
        {
            //Switch Idle animations ? Sounds ?
        }
    }
}