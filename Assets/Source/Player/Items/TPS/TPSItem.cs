using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class TPSItem : NetworkBehaviour
{
    [SerializeField] private PlayerCharacter _character;
    [SerializeField] private GameObject _tpsItem;

    // private void OnEnable()
    // {
    //     ;
    // }

    [SerializeField] private bool _initialized;
    private void Awake()
    {
        _character.ClientStarted += OnClientStarted;
    }

    private void OnEnable()
    {
        if (!_initialized)
            return;
        if (!base.IsOwner)
            return;
        OnClientStarted();
    }

    private void OnClientStarted()
    {
        if (!base.IsOwner)
            return;
        _initialized = true;
        _character.SetObjectEnableServer(_tpsItem, true);
    }

    private void OnDisable()
    {
        if (!base.IsOwner)
            return;
        _character.ClientStarted -= OnClientStarted;
        _character.SetObjectEnableServer(_tpsItem, false);
    }
}