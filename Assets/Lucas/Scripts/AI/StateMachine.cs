using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private State _currentState;

    public void ChangeState(State newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
    public State CurrentState() => _currentState;

    private void Update()
    {
        _currentState?.Update();
    }
}
