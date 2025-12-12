using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipableItem : MonoBehaviour
{
    [SerializeField] private GameObject _itemObject;
    [SerializeField] private AnimatorItemSwitcher _itemSwitcher;
    [SerializeField] private string _actionName;

    private void Start()
    {
        _itemSwitcher.Character.ClientStarted += OnClienStarted;
    }

    private void OnClienStarted()
    {
        _itemSwitcher.Character.Binds.FindAction(_actionName, true).performed += OnEquipActionPerformed;
    }

    private void OnEquipActionPerformed(InputAction.CallbackContext obj)
    {
        _itemSwitcher.SwitchItem(_itemObject);
    }

    private void OnDisable()
    {
        _itemSwitcher.Character.ClientStarted -= OnClienStarted;
        _itemSwitcher.Character.Binds.FindAction(_actionName, true).performed -= OnEquipActionPerformed;
    }
}