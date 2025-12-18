using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunState : EnemyState
{
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
    }
}
