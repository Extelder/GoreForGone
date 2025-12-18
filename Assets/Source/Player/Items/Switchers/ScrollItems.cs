using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScrollItems : MonoBehaviour
{
    [SerializeField] private float _scrollCooldown;
    
    [SerializeField] private PlayerCharacter _character;
    [SerializeField] private EquipableItem[] _equipableItems;

    private bool _canChange = true;

    private int _currentItemId = 0;

    private void OnEnable()
    {
        _character.ClientStarted += OnClientStarted;
    }

    private void OnClientStarted()
    {
        _character.Binds.Character.ScrollItems.started += OnScrollItemsStarted;
    }

    private void OnScrollItemsStarted(InputAction.CallbackContext obj)
    {
        if (_canChange == false)
            return;
        _canChange = false;

        float value = _character.Binds.Character.ScrollItems.ReadValue<float>();

        _currentItemId += value > 0 ? 1 : -1;

        if (_currentItemId < 0)
        {
            _currentItemId = _equipableItems.Length - 1;
        }

        if (_currentItemId > _equipableItems.Length - 1)
        {
            _currentItemId = 0;
        }


        _equipableItems[_currentItemId].Equip();
        StartCoroutine(RecoveringCanChange());
    }

    private IEnumerator RecoveringCanChange()
    {
        yield return new WaitForSeconds(_scrollCooldown);
        _canChange = true;
    }

    private void OnDisable()
    {
        _character.Binds.Character.ScrollItems.started -= OnScrollItemsStarted;
        _character.ClientStarted -= OnClientStarted;
    }
}