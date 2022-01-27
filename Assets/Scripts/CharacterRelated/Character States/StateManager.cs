using UnityEngine;
using UnityEngine.AI;

namespace Khynan_Coding
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class StateManager : MonoBehaviour
    {
        [HideInInspector] public bool CharacterIsMoving = false;

        private BasicState currentState;
        public BasicState CurrentState { get => currentState; set => currentState = value; }

        #region Public references
        public InteractionHandler InteractionHandler => GetComponent<InteractionHandler>();
        public NavMeshAgent NavMeshAgent => GetComponent<NavMeshAgent>();
        private CharacterController CharacterController => GetComponent<CharacterController>();
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