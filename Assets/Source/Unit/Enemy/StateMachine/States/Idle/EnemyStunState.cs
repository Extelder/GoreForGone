using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunState : EnemyState
{
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    public override void Enter()
    {
        if (!base.IsServer)
            return;
        EnemyAnimator.React();
        CanChanged = false;
    }

    public void ReactAnimationEnd()
    {
        CanChanged = true;
        _enemyStateMachine.ChaseLastDetectedCreature();
        Debug.Log("CHASE LAST");
    }
}
