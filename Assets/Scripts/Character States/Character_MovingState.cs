using UnityEngine;

namespace Khynan_Coding
{
    public class Character_MovingState : BasicState
    {
        InteractionHandler interactionHandler;
        Player_MovementController character_MovementHandler;

        public override void EnterState(StateManager stateManager)
        {
            interactionHandler = stateManager.InteractionHandler;
            character_MovementHandler = stateManager.GetComponent<Player_MovementController>();

            stateManager.CharacterIsMoving = true;
            stateManager.CharacterAnimationController.SetAnimationBoolean(
                stateManager.CharacterAnimationController.Animator, "IsMoving", stateManager.CharacterIsMoving);
            

            Helper.DebugMessage("Entering <MOVING> state", stateManager.transform); 
        }

        public override void ExitState(StateManager stateManager)
        {
            stateManager.CharacterIsMoving = false;
            stateManager.CharacterAnimationController.SetAnimationBoolean(
                stateManager.CharacterAnimationController.Animator, "IsMoving", stateManager.CharacterIsMoving);

            Helper.DebugMessage("Exiting <MOVING> state", stateManager.transform);
        }

        public override void ProcessState(StateManager stateManager)
        {
            if (character_MovementHandler && character_MovementHandler.DirectionToMove == Vector3.zero 
                && interactionHandler && !interactionHandler.HasATarget)
            {
                Helper.DebugMessage("Destination Reached !");
                stateManager.SwitchState(stateManager.IdleState);
                return;
            }

            HandleCharacterRotation(stateManager.NavMeshAgent, stateManager.transform);
        }

        public void HandleCharacterRotation(UnityEngine.AI.NavMeshAgent navMeshAgent, Transform transform)
        {
            if (navMeshAgent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                transform.rotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
            }
        }
    }
}