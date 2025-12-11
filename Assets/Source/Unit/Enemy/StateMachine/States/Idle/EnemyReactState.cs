using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReactState : EnemyState
{
    public override void Enter()
    {
        if (!base.IsServer)
            return;
        CanChanged = false;
    }

    public void ReactAnimationEnd()
    {
        CanChanged = true;
    }
}
