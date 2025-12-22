using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemSelector : MonoBehaviour
{
    [field: SerializeField] public Outline Outline { get; private set; }
    [SerializeField] private InventoryItem _inventoryItem;

    private void OnMouseEnter()
    {
        _inventoryItem.Select();
    }

    private void OnMouseExit()
    {
        _inventoryItem.DeSelect();
    }

    private void OnDisable()
    {
        _inventoryItem.DeSelect();
    }
}