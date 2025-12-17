using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeAddictableObject : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerCharacter.TimeValueChanged += OnTimeValueChanged;
    }

    protected abstract void OnTimeValueChanged(float value);


    private void OnDisable()
    {
        PlayerCharacter.TimeValueChanged -= OnTimeValueChanged;
    }
}