using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public abstract class PlayerSound : Sound
{
    [field: SerializeField] public bool MakeNoice = true;
    [ShowIf(nameof(MakeNoice))][field: SerializeField] public OverlapSettings OverlapSettings;
}
