using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyState
{
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    [SerializeField] private EnemyAttackState _enemyAttackState;
    [SerializeField] private EnemyNavMeshMove _enemyNavMeshMove;
    [SerializeField] private UnitPlayerDetector _unitPlayerDetector;
    
    [SerializeField] private float _lostTime;
    [SerializeField] private float _updateTargetRate;
    public Transform Target { get; private set; }
    private Coroutine _losingPlayerCoroutine;

    public override void OnStartClient()
    {
        if (!base.IsServer)
            return;
        base.OnStartClient();
        _unitPlayerDetector.PlayerLost += OnPlayerLost;
        _unitPlayerDetector.PlayerDetected += OnPlayerDetected;
    }

    public void ChangeTarget(Transform target)
    {
        if (!base.IsServer)
            return;
        Target = target;
        _enemyAttackState.AttackAnimationEnded += OnAttackAnimationEnded;
    }

    private void OnPlayerDetected()
    {
        StopCoroutine(_losingPlayerCoroutine);
    }

    private void OnPlayerLost()
    {
        _losingPlayerCoroutine = StartCoroutine(LosingPlayer());
    }

    private IEnumerator LosingPlayer()
    {
        yield return new WaitForSeconds(_lostTime);
        Exit();
        CanChanged = true;
        _enemyStateMachine.Patrol();
    }

    private void OnAttackAnimationEnded()
    {
        Enter();
    }

    public override void Enter()
    {
        if (!base.IsServer)
            return;
        StopAllCoroutines();
        StartCoroutine(Chasing());
        CanChanged = false;
    }

    public override void Exit()
    {
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        if (!base.IsServer)
            return;
        _enemyAttackState.AttackAnimationEnded -= OnAttackAnimationEnded;
        _unitPlayerDetector.PlayerLost -= OnPlayerLost;
        _unitPlayerDetector.PlayerDetected -= OnPlayerDetected;

        StopAllCoroutines();
    }
    
    private IEnumerator Chasing()
    {
        while (true)
        {
            CallAnimations();
            if (Target != null)
                _enemyNavMeshMove.SetDestinationServer(Target.position);
            yield return new WaitForSeconds(_updateTargetRate);
        }
    }
    
    public virtual void CallAnimations()
    {
        EnemyAnimator.Run();
    }

    public IEnumerator ChasingWithoutAnimation()
    {
        while (true)
        {
            _enemyNavMeshMove.SetDestinationServer(Target.position);
            yield return new WaitForSeconds(_updateTargetRate);
        }
    }
}