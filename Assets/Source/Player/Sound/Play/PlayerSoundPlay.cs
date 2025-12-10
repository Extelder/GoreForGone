using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(DrawSphere), typeof(AudioSource))]
public class PlayerSoundPlay : SoundPlay, INoiceMakeable
{
    public override void OnSoundPlayServer()
    {
    }

    public override void OnSoundPlayObserver()
    {
        if (!MakeNoice)
            return;
        SoundMakeNoise.MakeNoise(OverlapSettings);
    }

    [field: SerializeField]  public bool MakeNoice { get; set; }
    [field: SerializeField]  public OverlapSettings OverlapSettings { get; set; }
}
