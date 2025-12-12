using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAttackState : GunState
{
    public override void Enter()
    {
        Animator.Attack();
        CanChanged = false;
    }

    private void GunAttackEndAnimation()
    {
        CanChanged = true;
    }
}