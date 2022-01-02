using Khynan_Survival;
using UnityEngine;

public class Character_MovingState : BasicState
{
    public override void EnterState(StateManager stateManager)
    {
        UtilityClass.DebugMessage("Entering <MOVING> state", stateManager.GetComponent<Transform>());
        stateManager.CharacterIsMoving = true;
    }

    public override void ExitState(StateManager stateManager)
    {
        UtilityClass.DebugMessage("Exiting <MOVING> state", stateManager.GetComponent<Transform>());

        stateManager.CharacterIsMoving = false;
    }

    public override void ProcessState(StateManager stateManager)
    {
        InteractionHandler cursorDetection = stateManager.GetComponent<InteractionHandler>();
        Player_MovementController character_MovementHandler = stateManager.GetComponent<Player_MovementController>();

        if (character_MovementHandler.DirectionToMove == Vector3.zero && !cursorDetection.HasATarget)
        {
            UtilityClass.DebugMessage("Destination Reached !");
            stateManager.SwitchState(stateManager.IdleState);
        }
        else if (character_MovementHandler.DirectionToMove != Vector3.zero)
        {
            UpdateTransformRotation(character_MovementHandler, stateManager.transform);
        }
    }

    #region Updating character elements while moving - Rotation and moving animation
    //private void UpdateCharacterMovementAnimation(NavMeshAgent agent, Animator animator, string animationFloatName, float smoothTime)
    //{
    //    if (!agent.hasPath)
    //    {
    //        animator.SetFloat(animationFloatName, 0, smoothTime, Time.deltaTime);
    //        return;
    //    }

    //    float moveSpeed = agent.velocity.sqrMagnitude / agent.speed;
    //    animator.SetFloat(animationFloatName, moveSpeed, smoothTime, Time.deltaTime);
    //}

    private void UpdateTransformRotation(Player_MovementController stateManager, Transform _transform)
    {
        //This operation/conversion helps us rotate the character in the right direction relative to the movement direction and the camera's orientation.
        Quaternion targetRotation = Quaternion.LookRotation(
            (UtilityClass.GetMainCameraForwardDirection(0) * stateManager.DirectionToMove.z +
            UtilityClass.GetMainCameraRightDirection(0) * stateManager.DirectionToMove.x).normalized, Vector3.up);

        //This operation/conversion helps us rotate the character in the right direction relative to his movement.
        //Quaternion targetRotation = Quaternion.LookRotation(stateManager.DirectionToMove, Vector3.up.normalized);

        _transform.localRotation = Quaternion.RotateTowards(_transform.rotation, targetRotation, stateManager.RotationSpeed * Time.fixedDeltaTime);
    }
    #endregion
}