using UnityEngine;

public class StateMachine
{
    private IState currentState;

        public void ChangeState(IState newState)
    {
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
