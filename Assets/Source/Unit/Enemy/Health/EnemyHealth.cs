using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UniRx;
using Unity.Mathematics;
using UnityEngine;

public class EnemyHealth : Health
{
    public Vector3 HitPoint { get; set; }
    
    public override void Death()
    {}

    private void Update()
    {
        Debug.Log(CurrentValue);
    }

    public virtual void GetHitPoint(Vector3 hitPoint)
    {
        HitPoint = hitPoint;
    }
}