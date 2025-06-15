using UnityEngine;

public class StateMachine
{
    private IState currentState;

    public IState CurrentState => currentState;

        public void ChangeState(IState newState)
    {
        if (newState == currentState) return;

        //Debug.Log($"Switching from {currentState?.GetType().Name} to {newState.GetType().Name}");

        currentState?.Exit();
        //  ^^^ Shorthand for
        // if (currentState != null)
        //      currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}
