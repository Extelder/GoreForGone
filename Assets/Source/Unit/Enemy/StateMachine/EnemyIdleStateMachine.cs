using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleStateMachine : EnemyStateMachine
{
    [SerializeField] private EnemyState _idle;
    
    public void Idle()
    {
        ChangeState(_idle);
    }
}
