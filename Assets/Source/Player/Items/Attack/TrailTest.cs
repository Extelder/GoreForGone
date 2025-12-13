using System.Collections;
using System.Collections.Generic;
using Drakkar.GameUtils;
using UnityEngine;

public class TrailTest : MonoBehaviour
{
    [SerializeField] private DrakkarTrail _trail;

    public void StartTrail()
    {
        _trail.Begin();
    }

    public void StopTrail()
    {
        _trail.End();
    }
}