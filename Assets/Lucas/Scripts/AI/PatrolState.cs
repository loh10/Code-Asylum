using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{
    private readonly Transform[] _waypoints;
    private int _indexWaypoint;
    private Transform _currentWaypoint;
    
    private readonly NavMeshAgent _agent;
    
    private float _timer;
    private const float _waitTime = 3f;

    public PatrolState(NavMeshAgent agent, Transform[] waypoints)
    {
        _agent = agent;
        _waypoints = waypoints;
    }
    
    public override void Enter()
    {
        if (!_currentWaypoint)
            _currentWaypoint = _waypoints[_indexWaypoint];
        
        _agent.SetDestination(_currentWaypoint.position);
    }
    
    public override void Update()
    {
        if (Vector3.Distance(_agent.transform.position, _currentWaypoint.position) > 0.5f)
            return;
        
        _timer += Time.deltaTime;
        if (_timer < _waitTime)
            return;
        
        _timer = 0;
        _indexWaypoint++;
        if (_indexWaypoint >= _waypoints.Length)
            _indexWaypoint = 0;
        _currentWaypoint = _waypoints[_indexWaypoint];
        _agent.SetDestination(_currentWaypoint.position);
    }
    
    public override void Exit()
    {
        _timer = 0;
    }
    public void SetCurrentWaypoint(Vector3 currentWaypoint) => _currentWaypoint.position = currentWaypoint;
}
