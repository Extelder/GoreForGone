using System;
using System.Collections;
using System.Collections.Generic;
using MilkShake;
using UnityEngine;

public class TakeUpShake : MonoBehaviour
{
    [SerializeField] private Shaker _shaker;
    [SerializeField] private ShakePreset _shakePreset;

    private void OnEnable()
    {
        _shaker.Shake(_shakePreset);
    }
}