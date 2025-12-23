using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class PlayVfx : NetworkBehaviour
{
    [SerializeField] private ParticleSystem _vfx;
    
    [ServerRpc(RequireOwnership = false)]
    public void Play()
    {
        PlayObserver();
    }

    [ObserversRpc]
    private void PlayObserver()
    {
        _vfx.Play();
    }
}
