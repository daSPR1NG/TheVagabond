using UnityEngine;
using UnityEngine.AI;

namespace Khynan_Coding
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class StateManager : MonoBehaviour
    {
        [HideInInspector] public bool CharacterIsMoving = false;

        private CharacterState currentState;
        public CharacterState CurrentState { get => currentState; set => currentState = value; }

        #region Public references
        public InteractionHandler InteractionHandler => GetComponent<InteractionHandler>();
        public NavMeshAgent NavMeshAgent => GetComponent<NavMeshAgent>();
        public CharacterController CharacterController => GetComponent<CharacterController>();
        #endregion

        protected virtual void Update()
        {
            CurrentState.ProcessState(this);
        }

        protected void SetDefaultStateAtStart(CharacterState baseState)
        {
            CurrentState = baseState;
            CurrentState.EnterState(this);
        }

        public void SwitchState(CharacterState newState)
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