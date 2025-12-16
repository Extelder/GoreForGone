using System;
using System.Collections;
using System.Collections.Generic;
using EvolveGames;
using UnityEngine;

public class PlayerFOVChanger : MonoBehaviour
{
    [SerializeField] private PlayerController _controller;

    [SerializeField] private bool _recoverOnDisable;

    private float _defaultFOV;

    public void AddFOV(float value)
    {
        _controller.InstallFOV += value;
    }

    public void RecoverFOV()
    {
        _controller.InstallFOV = _controller.DefaultFOV;
    }

    private void OnDisable()
    {
        RecoverFOV();
    }
}