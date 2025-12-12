using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UniRx;
using Unity.Mathematics;
using UnityEngine;

public class EnemyHealth : Health
{
    public ReactiveProperty<bool> Dead { get; private set; } = new ReactiveProperty<bool>();

    public override void Death()
    {
        if (Dead.Value)
            return;
        Dead.Value = true;
    }
}