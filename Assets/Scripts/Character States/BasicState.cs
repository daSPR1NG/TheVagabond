namespace Khynan_Coding
{
    public abstract class BasicState
    {
        public abstract void EnterState(StateManager stateManager);

        public abstract void ProcessState(StateManager stateManager);

        public abstract void ExitState(StateManager stateManager);
    }
}