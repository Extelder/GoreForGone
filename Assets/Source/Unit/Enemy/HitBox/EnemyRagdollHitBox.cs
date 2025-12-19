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

    private Vector3 _hitPoint;
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

    private void OnEnemyDead()
    {
        _enemyRagdollDeath.Death(_hitPoint);
    }


    public override void OnHitWithRaycastServer(Vector3 point, Vector3 normal)
    {
        base.OnHitWithRaycastServer(point, normal);
        _hitPoint = point;
    }

    public override void OnHitServer(Vector3 overlapCenter)
    {
        base.OnHitServer(overlapCenter);
        _hitPoint = overlapCenter;
    }

    private void OnDisable()
    {
        if (!base.IsServer)
            return;
        _disposable.Clear();
    }
}