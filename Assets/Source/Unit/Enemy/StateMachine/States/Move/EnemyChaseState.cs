using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyState
{
    [SerializeField] private LookAtClosestPlayer _lookAtClosestPlayer;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    [SerializeField] private EnemyNavMeshMove _enemyNavMeshMove;
    [SerializeField] private UnitPlayerDetector _unitPlayerDetector;
    [SerializeField] private float _chaseSpeedMultiplier;
    
    [SerializeField] private float _updateTargetRate;
    public Transform Target { get; private set; }
    private float _defaultSpeed;
    private Coroutine _losingPlayerCoroutine;

    public override void OnStartClient()
    {
        if (!base.IsServer)
            return;
        base.OnStartClient();
        _defaultSpeed = _agent.speed;
        _unitPlayerDetector.PlayerLost += OnPlayerLost;
    }

    public void ChangeTarget(Transform target)
    {
        if (!base.IsServer)
            return;
        Target = target;
    }

    private void OnPlayerLost()
    {
        CanChanged = true;
        _enemyStateMachine.Patrol();
    }

    public override void Enter()
    {
        if (!base.IsServer)
            return;
        _lookAtClosestPlayer.StartLookAt();
        _agent.speed *= _chaseSpeedMultiplier;
        StopAllCoroutines();
        StartCoroutine(Chasing());
        CanChanged = false;
    }

    public override void Exit()
    {
        _agent.speed = _defaultSpeed;
        _lookAtClosestPlayer.StopLookAt();
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        if (!base.IsServer)
            return;
        _unitPlayerDetector.PlayerLost -= OnPlayerLost;
        StopAllCoroutines();
    }
    
    private IEnumerator Chasing()
    {
        while (true)
        {
            OnChasing();
            if (Target != null)
                _enemyNavMeshMove.SetDestinationServer(Target.position);
            yield return new WaitForSeconds(_updateTargetRate);
        }
    }

    public virtual void OnChasing()
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