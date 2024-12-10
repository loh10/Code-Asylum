using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private Camera _agentCamera;

    private NavMeshAgent _agent;
    private StateMachine _stateMachine;
    
    private Camera _mainCamera;
    private Transform _agentTransform;
    
    //State
    private IdleState _idleState;
    private FollowState _followState;
    private PatrolState _patrolState;

    SaveReload saveReload;
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _idleState = new IdleState(_agent);
        _followState = new FollowState(_target, _agent);
        _patrolState = new PatrolState(_agent, _waypoints);
        
        _stateMachine = GetComponent<StateMachine>();
        if (_stateMachine == null)
        {
            _stateMachine = gameObject.AddComponent<StateMachine>();
        }
        _stateMachine.ChangeState(_patrolState);
        
        _mainCamera = Camera.main;
        _agentTransform = transform;
    }

    private void Update()
    {
        bool isAgentVisible = IsVisible(_agentTransform, _mainCamera);
        bool isTargetVisible = IsVisible(_target, _agentCamera);
        
        if (isAgentVisible)
        {
            if (isTargetVisible)
            {
                _patrolState.SetCurrentWaypoint(_target.position);
            }
            if (_stateMachine.CurrentState() is not IdleState)
                _stateMachine.ChangeState(_idleState);
        }
        else if (isTargetVisible)
        {
            if (_stateMachine.CurrentState() is not FollowState)
                _stateMachine.ChangeState(_followState);
        }
        else 
        {
            if (_stateMachine.CurrentState() is FollowState)
                _patrolState.SetCurrentWaypoint(_target.position);
            if (_stateMachine.CurrentState() is not PatrolState)
                _stateMachine.ChangeState(_patrolState);
        }
    }

    private static bool IsVisible(Transform target, Camera cam)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
        bool isVisible = planes.All(plane => plane.GetDistanceToPoint(target.position) > -0.5f);
        
        if (!isVisible) return false;
        
        Vector3 direction = target.position - cam.transform.position;
        if (!Physics.Raycast(cam.transform.position, direction.normalized, out RaycastHit hit, Mathf.Infinity)) return false;
        
        return hit.transform == target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            saveReload.IsDead();
            return;
        }
    }
}