using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeStateMachine : EnemyStateMachine
{
    [SerializeField] private EnemyState _shoot;
    [SerializeField] private EnemyState _moveToRandomPoint;
    
    public void Shoot()
    {
        if (!base.IsServer)
            return;
        if (CurrentState == _shoot)
            return;
        CurrentState.CanChanged = true;
        ChangeState(_shoot);
    }

    public void MoveToRandomPoint()
    {
        if (!base.IsServer)
            return;
        ChangeState(_moveToRandomPoint);
    }
}
