using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunReloadState : GunState
{
    public override void Enter()
    {
        Animator.Reload();
        CanChanged = false;
    }
    private void GunReloadEndAnimation()
    {
        CanChanged = true;
    }
}
