using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSoundPlayAndMix : PlayerSoundPlay
{
    [SerializeField] private AudioClip[] _clips;
    private int _currentClip;
    
    public override void OnSoundPlayServer()
    {
        _currentClip = Random.Range(0, _clips.Length);
        base.OnSoundPlayServer();
    }

    public override void OnSoundPlayObserver()
    {
        Source.clip = _clips[_currentClip];
        base.OnSoundPlayObserver();
    }
}