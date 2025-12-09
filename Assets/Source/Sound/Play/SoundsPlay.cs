using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class SoundsPlay : Sound
{
    [ServerRpc(RequireOwnership = false)]
    public void SoundPlayServer()
    {
        SoundPlayObserver();
    }

    [ObserversRpc]
    public void SoundPlayObserver()
    {
        Source?.Play();
        StartCoroutine(WaitToDespawn());
    }
}
