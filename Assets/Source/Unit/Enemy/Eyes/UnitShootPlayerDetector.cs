using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShootPlayerDetector : UnitPlayerDetector
{
    [SerializeField] private EnemyRangeStateMachine _enemyRangeStateMachine;
    [SerializeField] private float _shootDistance;

    public override void Chase(RaycastHit hit)
    {
        if (hit.distance <= _shootDistance)
        {
            _enemyRangeStateMachine.Shoot();
            return;
        }
        base.Chase(hit);
    }
}
