using UnityEngine;

namespace Khynan_Coding
{
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

        #region Public references
        public CharacterAnimationController CharacterAnimationController => GetComponent<CharacterAnimationController>();
        public InteractionHandler InteractionHandler => GetComponent<InteractionHandler>();
        public UnityEngine.AI.NavMeshAgent NavMeshAgent => GetComponent<UnityEngine.AI.NavMeshAgent>();
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
}