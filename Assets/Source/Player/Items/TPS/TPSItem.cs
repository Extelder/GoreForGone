using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class TPSItem : NetworkBehaviour
{
    [SerializeField] private GameObject _tpsItem;

    private void OnEnable()
    {
        if (base.IsOwner)
            PlayerCharacter.Instance?.SetObjectEnableServer(_tpsItem, true);

        _tpsItem.SetActive(true);
    }

    private void OnDisable()
    {
        if (base.IsOwner)
            PlayerCharacter.Instance?.SetObjectEnableServer(_tpsItem, false);

        _tpsItem.SetActive(false);
    }
}