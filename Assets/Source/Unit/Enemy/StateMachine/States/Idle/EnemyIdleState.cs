using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyIdleState : EnemyState
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    [SerializeField] private float _maxRandomTimeToStartPatrolling;
    private CompositeDisposable _disposable = new CompositeDisposable();

    public override void Enter()
    {
        if (!base.IsServer)
            return;
        StopAllCoroutines();
        _agent.isStopped = true;
        StartCoroutine(Idling());
        Debug.Log("IDLE");
    }

    private IEnumerator Idling()
    {
        yield return new WaitForSeconds(0.2f);
        EnemyAnimator.Idle();
        yield return new WaitForSeconds(Random.Range(0, _maxRandomTimeToStartPatrolling));
        _enemyStateMachine.Patrol();
    }

    public override void Exit()
    {
        StopAllCoroutines();
        _disposable.Clear();
        _agent.isStopped = false;
        base.Exit();
    }

    private void OnDisable()
    {
        if (!base.IsServer)
            return;
        _disposable.Clear();
    }
}