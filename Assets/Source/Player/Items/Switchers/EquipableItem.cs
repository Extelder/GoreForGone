using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipableItem : MonoBehaviour
{
    [SerializeField] protected GameObject itemObject;
    [SerializeField] protected AnimatorItemSwitcher _itemSwitcher;
    [SerializeField] private string _actionName;

    private void Start()
    {
        _itemSwitcher.Character.ClientStarted += OnClienStarted;
    }

    private void OnClienStarted()
    {
        _itemSwitcher.Character.Binds.FindAction(_actionName, true).performed += OnEquipActionPerformed;
    }

    protected virtual void OnEquipActionPerformed(InputAction.CallbackContext obj)
    {
        if (_itemSwitcher.GetCurrentItem() != itemObject)
            _itemSwitcher.SwitchItem(itemObject);
    }

    private void OnDisable()
    {
        _itemSwitcher.Character.ClientStarted -= OnClienStarted;
        _itemSwitcher.Character.Binds.FindAction(_actionName, true).performed -= OnEquipActionPerformed;
    }
}