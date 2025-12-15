using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseShootState : EnemyChaseState
{
    [SerializeField] private UnitShootPlayerDetector _unitShootPlayerDetector;
    [SerializeField] private EnemyRangeStateMachine _enemyRangeStateMachine;
    
    public override void OnChasing()
    {
        base.OnChasing();
        if (_unitShootPlayerDetector.CanShootNow)
        {
            CanChanged = true;
            _enemyRangeStateMachine.Shoot();
        }
    }
}
