using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickInventoryItem : MonoBehaviour
{
    [SerializeField] private Outline[] _outlines;
    [SerializeField] private GameObject _rebindCanvas;
    [SerializeField] private EquipableItem _equipableItem;

    [SerializeField] private GameObject _activeRebindObject;

    private void OnMouseDown()
    {
        if (!_activeRebindObject.activeInHierarchy)
            Equip();
    }

    private void OnMouseEnter()
    {
        SetOutline(true);
        _rebindCanvas.SetActive(true);
    }

    public void SetOutline(bool value)
    {
        for (int i = 0; i < _outlines.Length; i++)
        {
            _outlines[i].enabled = value;
        }
    }

    private void OnMouseExit()
    {
        SetOutline(false);
        _rebindCanvas.SetActive(false);
    }

    public void Equip()
    {
        _equipableItem.Equip();
    }
}