using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using Unity.Mathematics;
using UnityEngine;

public class EnemyHitBox : UnitHitBox
{
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    
    public override void OnHitWithRaycastServer()
    {
        base.OnHitWithRaycastServer();
        _enemyStateMachine.React();
    }

    public override void OnHitServer()
    {
        base.OnHitServer();
        _enemyStateMachine.React();
    }
}