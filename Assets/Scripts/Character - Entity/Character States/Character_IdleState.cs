using Khynan_Survival;
using UnityEngine;
using UnityEngine.AI;

public class Character_IdleState : BasicState
{
    public override void EnterState(StateManager stateManager)
    {
        UtilityClass.DebugMessage("Entering <IDLE> state", stateManager.GetComponent<Transform>());

        UtilityClass.ResetAgentDestination(stateManager.GetComponent<NavMeshAgent>());
    }

    public override void ExitState(StateManager stateManager)
    {

        UtilityClass.DebugMessage("Exiting <IDLE> state", stateManager.GetComponent<Transform>());
    }

    public override void ProcessState(StateManager stateManager)
    {
        //Switch Idle animations ? Sounds ?
    }
}
