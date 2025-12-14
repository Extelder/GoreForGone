using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveToRandomPointState : EnemyState
{
    [SerializeField] private LookAtClosestPlayer _lookAtClosestPlayer;
    [SerializeField] private bool _canShoot;
    [ShowIf(nameof(_canShoot))] [SerializeField]
    private EnemyRangeStateMachine _enemyRangeStateMachine;
    [ShowIf(nameof(_canShoot))] [SerializeField] private UnitShootPlayerDetector _unitPlayerDetector;
    [SerializeField] private EnemyNavMeshMove _enemyNavMeshMove;
    [SerializeField] private float _randomPointRange;
    [SerializeField] private NavMeshAgent _agent;
    
    public override void Enter()
    {
        StartCoroutine(MovingToRandomPoint());
        _lookAtClosestPlayer.StartLookAt();
        CanChanged = false;
        EnemyAnimator.Move();
    }

    public override void Exit()
    {
        StopAllCoroutines();
        _lookAtClosestPlayer.StopLookAt();
        base.Exit();
    }

    private IEnumerator MovingToRandomPoint()
    {
        while (true)
        {
            if (GetRandomPointOnNavMesh(transform.position, _randomPointRange, out Vector3 point))
            {
                EnemyAnimator.Move();
                _enemyNavMeshMove.SetDestinationServer(point);
            }

            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(() => AgentReachedDestination());
            CanChanged = true;
            if (!_unitPlayerDetector.Detected)
            {
                _enemyRangeStateMachine.Patrol();
                break;
            }
            if (!_unitPlayerDetector.CanShootNow)
            {
                _enemyRangeStateMachine.ChaseLastDetectedCreature();
                break;
            }
            if (_canShoot)
                _enemyRangeStateMachine.Shoot();
        }
    }

    public bool GetRandomPointOnNavMesh(Vector3 center, float radius, out Vector3 result)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        Vector3 randomPoint = center + randomDirection;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private bool AgentReachedDestination() => _agent.remainingDistance <= 0.1f;
}
