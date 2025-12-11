using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public abstract class SoundPlay : Sound
{
    [ServerRpc(RequireOwnership = false)]
    public void SoundPlayServer()
    {
        OnSoundPlayServer();
        SoundPlayObserver();
    }

    [ObserversRpc]
    public void SoundPlayObserver()
    {
        if (Source == null)
            return;
        if (Source.isPlaying)
            return;
        OnSoundPlayObserver();
        Source?.Play();
        StartCoroutine(WaitToDespawn());
    }

    [ServerRpc(RequireOwnership = false)]
    public void SoundStopServer()
    {
        SoundStopObserver();
    }

    [ObserversRpc]
    public void SoundStopObserver()
    {
        if (Source == null)
            return;
        Source?.Stop();
    }

    public abstract void OnSoundPlayServer();
    public abstract void OnSoundPlayObserver();
}