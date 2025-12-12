using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using Unity.Mathematics;
using UnityEngine;

public class EnemyHitBox : UnitHitBox
{
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    
    public override void OnHitWithRaycastServer(Vector3 point, Vector3 normal)
    {
        base.OnHitWithRaycastServer(point, normal);
        _enemyStateMachine.React();
    }

    public override void OnHitServer(Transform overlapCenter)
    {
        base.OnHitServer(overlapCenter);
        _enemyStateMachine.React();
    }
}