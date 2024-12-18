using UnityEngine;
using UnityEngine.AI;

public class FollowState : State
{
    private readonly Transform _target;
    private readonly NavMeshAgent _agent;
    private const float _speedMultiplier = 1.25f;
    private const float _distanceToTarget = 2f;
    
    public FollowState(Transform target, NavMeshAgent agent)
    {
        _target = target;
        _agent = agent;
    }

    public override void Enter()
    {
        _agent.speed *= _speedMultiplier;
    }
    public override void Update()
    {
        _agent.SetDestination(_target.position);

        if (Vector3.Distance(_target.position, _agent.transform.position) < _distanceToTarget)
        {
            _target.gameObject.GetComponent<SaveReload>().IsDead();
        }
    }
    public override void Exit()
    {
        _agent.speed /= _speedMultiplier;
    }
}
