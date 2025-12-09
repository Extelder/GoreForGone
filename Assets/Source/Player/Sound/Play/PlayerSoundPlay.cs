using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class PlayerSoundPlay : PlayerSound
{
    [ServerRpc(RequireOwnership = false)]
    public void SoundPlayServer()
    {
        SoundPlayObserver();
    }

    [ObserversRpc]
    public void SoundPlayObserver()
    {
        if (MakeNoice)
            SoundMakeNoise.MakeNoise(OverlapSettings);
        Source?.Play();
        StartCoroutine(WaitToDespawn());
    }
}
