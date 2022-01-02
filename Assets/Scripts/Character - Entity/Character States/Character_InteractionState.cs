using Khynan_Survival;
using UnityEngine;

public class Character_InteractionState : BasicState
{
    public override void EnterState(StateManager stateManager)
    {
        //Play interaction animation 
        //Launch interaction function from the interacted object
        UtilityClass.DebugMessage("Entering <INTERACTION> state", stateManager.GetComponent<Transform>());
    }

    public override void ExitState(StateManager stateManager)
    {
        UtilityClass.DebugMessage("Exiting <INTERACTION> state", stateManager.GetComponent<Transform>());
    }

    public override void ProcessState(StateManager stateManager)
    {
        //Switch Idle animations ? Sounds ?
    }
}
