using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShootState : EnemyState
{
    [SerializeField] private EnemyRangeStateMachine _enemyRangeStateMachine;
    [SerializeField] private LookAtClosestPlayerNotIK _lookAtClosestPlayerNotIK;
    [SerializeField] private LookAtClosestPlayer _lookAtClosestPlayer;
    [SerializeField] private bool _stopNavMesh = true;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private GameObject _objectToSpawn;
    [SerializeField] private Transform _shootOrigin;

    public event Action ShootAnimationEnded;

    public override void Enter()
    {
        if (!base.IsServer)
            return;
        CanChanged = false;
        Debug.Log("START SHOOTING");
        EnemyAnimator.Shoot();
        _agent.isStopped = _stopNavMesh;
        _agent.enabled = false;
        _lookAtClosestPlayerNotIK.StartLookAt();
        _lookAtClosestPlayer.StartLookAt();
    }
    
    public void PerformShoot()
    {
        if (!base.IsServer)
            return;
        PlayerCharacter.Instance.ServerSpawnObject(_objectToSpawn, _shootOrigin.position, _shootOrigin.rotation);
    }

    public void ShootAnimationEnd()
    {
        if (!base.IsServer)
            return;
        if (_stopNavMesh)
            _agent.isStopped = false;
        CanChanged = true;
        ShootAnimationEnded?.Invoke();
        _enemyRangeStateMachine.MoveToRandomPoint();
    }

    public override void Exit()
    {
        _lookAtClosestPlayerNotIK.StopLookAt();
        _lookAtClosestPlayer.StopLookAt();
        _agent.enabled = true;
        Debug.Log("STOP SHOOTING");
    }
}
