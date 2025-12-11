using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolAndIdleState : EnemyPatrolState
{
    [SerializeField] private EnemyIdleStateMachine enemyIdleStateMachine;
    
    public override void OnDestinationReached()
    {
        enemyIdleStateMachine.Idle();
    }
}
