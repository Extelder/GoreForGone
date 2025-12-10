using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSphere : MonoBehaviour
{
    [SerializeField]
    private PlayerSoundPlay playerSoundPlay;

    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(playerSoundPlay.OverlapSettings.Origin.position, playerSoundPlay.OverlapSettings.SphereRadius);
    }
}