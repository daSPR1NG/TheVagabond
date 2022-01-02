using Khynan_Survival;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    #region Character States
    private BasicState currentState;
    public Character_IdleState IdleState = new();
    public Character_MovingState MovingState = new();
    public Character_InteractionState InteractionState = new();
    [HideInInspector] public bool CharacterIsMoving = false;

    public BasicState CurrentState { get => currentState; set => currentState = value; }
    #endregion

    protected virtual void Update()
    {
        CurrentState.ProcessState(this);
    }

    protected void SetDefaultStateAtStart(BasicState baseState)
    {
        CurrentState = baseState;
        CurrentState.EnterState(this);
    }

    public void SwitchState(BasicState newState)
    {
        if (CurrentState != newState)
        {
            CurrentState.ExitState(this);

            CurrentState = newState;
            newState.EnterState(this);
        }
    }
}
