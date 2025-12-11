using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public interface INoiceMakeable
{
    public bool MakeNoice { get; set; }
    public OverlapSettings OverlapSettings { get; set; }
}
