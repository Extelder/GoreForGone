using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipableItem : NetworkBehaviour
{
    [SerializeField] protected GameObject itemObject;
    [SerializeField] protected AnimatorItemSwitcher _itemSwitcher;
    [SerializeField] private string _actionName;

    private bool _initialized;

    private void Awake()
    {
        _itemSwitcher.Character.ClientStarted += OnClienStarted;
    }

    private void OnClienStarted()
    {
        if (!_itemSwitcher.Character.IsOwner)
            return;
        _initialized = true;
        _itemSwitcher.Character.Binds.FindAction(_actionName, true).performed += OnEquipActionPerformed;
        Debug.LogError("Subsribed");
    }

    protected virtual void OnEquipActionPerformed(InputAction.CallbackContext obj)
    {
        if (itemObject.activeInHierarchy == false)
            _itemSwitcher.SwitchItem(itemObject);
    }

    private void OnDisable()
    {
        if (!_itemSwitcher.Character.IsOwner)
            return;
        Debug.LogError("Decribe");
        _itemSwitcher.Character.ClientStarted -= OnClienStarted;
        _itemSwitcher.Character.Binds.FindAction(_actionName, true).performed -= OnEquipActionPerformed;
    }
}