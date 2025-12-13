using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShootState : EnemyState
{
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
        EnemyAnimator.Shoot();
        _agent.isStopped = _stopNavMesh;
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
    }
}
