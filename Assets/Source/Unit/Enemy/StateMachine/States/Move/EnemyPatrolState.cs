using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyState
{
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private EnemyNavMeshMove _enemyNavMeshMove;
    [SerializeField] private float _destinationReachThreshhold;
    [SerializeField] private NavMeshAgent _agent;

    private int _pointIndex;

    public override void Enter()
    {
        if (!base.IsServer)
            return;
        StopAllCoroutines();
        StartCoroutine(Patroling());
    }

    public override void Exit()
    {
        if (!base.IsServer)
            return;
        StopAllCoroutines();
    }

    private IEnumerator Patroling()
    {
        EnemyAnimator.Move();
        while (true)
        {
            _enemyNavMeshMove.SetDestinationServer(_patrolPoints[_pointIndex].position);
            yield return new WaitForSeconds(0.2f);
            yield return new WaitUntil(() => AgentReachedDestination());
            OnDestinationReached();
            _pointIndex++;
            if (_pointIndex >= _patrolPoints.Length)
            {
                _pointIndex = 0;
            }
        }
    }

    public virtual void OnDestinationReached()
    {
        EnemyAnimator.Idle();
    }

    private bool AgentReachedDestination() => _agent.remainingDistance <= _destinationReachThreshhold;
}