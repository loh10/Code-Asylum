using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private State currentState;

    public void ChangeState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
    public State CurrentState() => currentState;

    private void Update()
    {
        currentState?.Update();
    }
}
