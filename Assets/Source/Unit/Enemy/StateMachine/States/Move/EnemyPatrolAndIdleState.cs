using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolAndIdleState : EnemyPatrolState
{
    [SerializeField] private EnemyIdleStateMachine _enemyIdleStateMachine;
    
    public override void OnDestinationReached()
    {
        _enemyIdleStateMachine.Idle();
    }
}
