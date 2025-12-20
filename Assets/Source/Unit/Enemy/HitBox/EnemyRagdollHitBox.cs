using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using NTC.Global.System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyRagdollHitBox : EnemyHitBox
{
    private Vector3 _hitPoint;

    public override void OnHitWithRaycastServer(Vector3 point, Vector3 normal)
    {
        base.OnHitWithRaycastServer(point, normal);
        SetHealthHitPoint(point);
    }

    public override void OnHitServer(Vector3 overlapCenter)
    {
        base.OnHitServer(overlapCenter);
        SetHealthHitPoint(overlapCenter);
    }

    private void SetHealthHitPoint(Vector3 hitPoint)
    {
        EnemyHealth.GetHitPoint(hitPoint);
    }
}