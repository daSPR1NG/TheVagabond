using UnityEngine;
using UnityEngine.AI;

namespace Khynan_Coding
{
    public class Character_IdleState : BasicState
    {
        public override void EnterState(StateManager stateManager)
        {
            Helper.DebugMessage("Entering <IDLE> state", stateManager.GetComponent<Transform>());

            Helper.ResetAgentDestination(stateManager.GetComponent<NavMeshAgent>());
        }

        public override void ExitState(StateManager stateManager)
        {

            Helper.DebugMessage("Exiting <IDLE> state", stateManager.GetComponent<Transform>());
        }

        public override void ProcessState(StateManager stateManager)
        {
            //Switch Idle animations ? Sounds ?
        }
    }
}