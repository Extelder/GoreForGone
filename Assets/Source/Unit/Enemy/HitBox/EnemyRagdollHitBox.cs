using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using NTC.Global.System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyRagdollHitBox : EnemyHitBox
{
    [SerializeField] private EnemyRagdollDeath _enemyRagdollDeath;
    
    private Transform _overlapCenter;
    private Vector3 _hitPoint;
    private bool _hittedByRaycast;

    private CompositeDisposable _disposable = new CompositeDisposable();
    
    
    public override void OnStartClient()
    {
        if (!base.IsServer)
            return;
        base.OnStartClient();
        EnemyHealth.Dead.Subscribe(_ =>
        {
            if (EnemyHealth.Dead.Value)
            {
                OnEnemyDead();
            }
        }).AddTo(_disposable);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Hit(transform, 50, transform.position);
        }
    }

    private void OnEnemyDead()
    {
        if (_hittedByRaycast)
        {
            _enemyRagdollDeath.Death(_hitPoint);
            return;
        }
        _enemyRagdollDeath.Death(_overlapCenter);
    }
    

    public override void OnHitWithRaycastServer(Vector3 point, Vector3 normal)
    {
        base.OnHitWithRaycastServer(point, normal);
        _hittedByRaycast = true;
        _hitPoint = point;
    }

    public override void OnHitServer(Transform overlapCenter)
    {
        base.OnHitServer(overlapCenter);
        _hittedByRaycast = false;
        _overlapCenter = overlapCenter;
    }

    private void OnDisable()
    {
        if (!base.IsServer)
            return;
        _disposable.Clear();
    }
}
