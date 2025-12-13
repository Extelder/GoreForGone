using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeIdleStateMachine : EnemyRangeStateMachine
{
    [SerializeField] private EnemyState _idle;
    
    public void Idle()
    {
        if (!base.IsServer)
            return;
        ChangeState(_idle);
    }
}
