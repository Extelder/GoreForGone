using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class UnitShootPlayerDetector : UnitPlayerDetector
{
    [SerializeField] private EnemyRangeStateMachine _enemyRangeStateMachine;
    [SerializeField] private float _shootDistance;
    
    public bool CanShootNow { get; private set; }
    
    public override void Chase(RaycastHit hit)
    {
        if (hit.distance <= _shootDistance)
        {
            _enemyRangeStateMachine.Shoot();
            CanShootNow = true;
            return;
        }
        CanShootNow = false;
        base.Chase(hit);
    }
}
