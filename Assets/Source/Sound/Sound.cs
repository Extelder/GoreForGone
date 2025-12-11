using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public abstract class Sound : NetworkBehaviour
{
    [field: SerializeField] public AudioSource Source { get; set; }
    [field: SerializeField] public bool DestroyAfterPlay { get; private set; }

    public IEnumerator WaitToDespawn()
    {
        if (!DestroyAfterPlay)
            yield break;
        yield return new WaitForSeconds(Source.clip.length);
    }
}
