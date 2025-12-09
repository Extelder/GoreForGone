using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public static class SoundMakeNoise
{
    public static void MakeNoise(OverlapSettings overlapSettings)
    {
        overlapSettings.Colliders = new Collider[overlapSettings.Size];
       Physics.OverlapSphereNonAlloc(overlapSettings.Origin.position, overlapSettings.SphereRadius, overlapSettings.Colliders,
            overlapSettings.LayerMask);

        for (int i = 0; i < overlapSettings.Colliders.Length; i++)
        {
            if (overlapSettings.Colliders[i] == null)
            {
                continue;
            }

            if (overlapSettings.Colliders[i].TryGetComponent<EnemyHear>(out EnemyHear hear))
            {
                hear.SoundHeared(overlapSettings.Origin);
                return;
            }
        }
    }
}