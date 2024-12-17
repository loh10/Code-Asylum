using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : State
{
    private readonly NavMeshAgent _agent;
    private Animator _animation;
    
    public IdleState(NavMeshAgent agent)
    {
        _animation = agent.GetComponent<AIController>().GetAnimations();
        _agent = agent;
    }

    public override void Enter()
    {
        _animation.speed = 0;
        _agent.ResetPath();
    }
    public override void Update()
    {
        
    }
    public override void Exit()
    {
        _animation.speed = 1;
    }
}
