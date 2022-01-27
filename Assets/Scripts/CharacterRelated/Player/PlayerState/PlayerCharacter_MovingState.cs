using UnityEngine;

namespace Khynan_Coding
{
    public class PlayerCharacter_MovingState : BasicState
    {
        InteractionHandler interactionHandler;
        PlayerController playerController;


        public override void Init(StateManager stateManager)
        {
            interactionHandler = stateManager.InteractionHandler;
            playerController = (PlayerController)stateManager.GetComponent<CharacterController>();
        }

        public override void EnterState(StateManager stateManager)
        {
            Init(stateManager);

            stateManager.CharacterIsMoving = true;
            AnimatorHelper.SetAnimatorBooleanParameter(playerController.Animator, "IsMoving", stateManager.CharacterIsMoving);
            
            Helper.DebugMessage("Entering <MOVING> state", stateManager.transform); 
        }

        public override void ExitState(StateManager stateManager)
        {
            stateManager.CharacterIsMoving = false;
            AnimatorHelper.SetAnimatorBooleanParameter(playerController.Animator, "IsMoving", stateManager.CharacterIsMoving);

            Helper.DebugMessage("Exiting <MOVING> state", stateManager.transform);
        }

        public override void ProcessState(StateManager stateManager)
        {
            base.ProcessState(stateManager);

            if (playerController.DirectionToMove == Vector3.zero && !stateManager.NavMeshAgent.hasPath)
            {
                Helper.DebugMessage("Destination Reached !");
                stateManager.SwitchState(playerController.IdleState);
                return;
            }

            playerController.UpdateCharacterNavMeshAgentRotation(stateManager.NavMeshAgent, stateManager.transform, playerController.RotationSpeed);

            AnimatorHelper.SetAnimatorFloatParameter(
                playerController.Animator, "CharacterSpeed", playerController.MovementSpeed / 4, 1f);
        }
    }
}