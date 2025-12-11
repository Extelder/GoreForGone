using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    [SerializeField] private float _maxRandomTimeToStartPatrolling;

    public override void Enter()
    {
        StartCoroutine(Idling());
    }

    private IEnumerator Idling()
    {
        EnemyAnimator.Idle();
        yield return new WaitForSeconds(Random.Range(0, _maxRandomTimeToStartPatrolling));
        _enemyStateMachine.Patrol();
    }

    public override void Exit()
    {
        StopAllCoroutines();
        base.Exit();
    }
}