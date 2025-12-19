using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReactState : EnemyState
{
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    
    public override void Enter()
    {
        if (!base.IsServer)
            return;
        CanChanged = false;
        EnemyAnimator.React();
    }

    public void ReactAnimationEnd()
    {
        CanChanged = true;
        _enemyStateMachine.ChaseLastDetectedCreature();
        Debug.Log("CHASE LAST");
    }
}
