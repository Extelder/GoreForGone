using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class EnemyPatrolAndIdleState : EnemyPatrolState
{
    [SerializeField] private EnemyStateMachine _stateMachine;
    
    public override void OnDestinationReached()
    {
        _stateMachine.Idle();
    }
}
