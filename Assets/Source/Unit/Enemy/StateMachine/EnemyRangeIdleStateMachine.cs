using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeIdleStateMachine : EnemyRangeStateMachine
{
    [SerializeField] private EnemyState _idle;
    
    public override void Idle()
    {
        if (!base.IsServer)
            return;
        ChangeState(_idle);
    }
}
