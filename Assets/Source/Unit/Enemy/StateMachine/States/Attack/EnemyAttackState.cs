using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyState
{
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    [SerializeField] private LookAtClosestPlayer _lookAtClosestPlayer;
    [SerializeField] private EnemyChaseState _chaseState;
    [SerializeField] private bool _stopNavMesh = true;
    [SerializeField] private NavMeshAgent _agent;
    [field: SerializeField] public EnemyDamage Damage { get; private set; }

    public PlayerHitBox PlayerHitBox { get; private set; }

    public override void Enter()
    {
        if (!base.IsServer)
            return;
        _lookAtClosestPlayer.StartLookAt();
        CanChanged = false;
        Debug.Log("ATTACK");
        EnemyAnimator.Attack();
        _agent.isStopped = _stopNavMesh;
        if (!_agent.isStopped)
            StartCoroutine(_chaseState.ChasingWithoutAnimation());
    }

    public override void Exit()
    {
        _lookAtClosestPlayer.StopLookAt();
        _chaseState.StopAllCoroutines();
    }

    public void PerformAttack()
    {
        if (!base.IsServer)
            return;
        PlayerHitBox?.TakeDamage(Damage.GetDamage());
    }

    public virtual void OnPlayerDetected(PlayerHitBox hitBox)
    {
        if (!base.IsServer)
            return;
        PlayerHitBox = hitBox;
    }

    public void AttackAnimationEnd()
    {
        if (!base.IsServer)
            return;
        if (_stopNavMesh)
            _agent.isStopped = false;
        CanChanged = true;
        _enemyStateMachine.ChaseLastDetectedCreature();
    }
}