using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using NTC.Global.System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyRagdollHealth : EnemyHealth
{
    [SerializeField] private EnemyRagdollDeath _enemyRagdollDeath;

    public override void Death()
    {
        base.Death();
        _enemyRagdollDeath.Death(HitPoint);
    }
}