using UnityEngine.AI;

public class IdleState : State
{
    private readonly NavMeshAgent _agent;
    
    public IdleState(NavMeshAgent agent)
    {
        _agent = agent;
    }

    public override void Enter()
    {
        _agent.ResetPath();
    }
    public override void Update()
    {
        
    }
    public override void Exit()
    {
        
    }
}
