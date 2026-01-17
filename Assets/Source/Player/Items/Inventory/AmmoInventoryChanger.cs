using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AmmoInventoryChanger : MonoBehaviour
{
    [SerializeField] private GunAmmo _ammo;
    [SerializeField] private InventoryItem _inventoryItem;
    
    private void OnEnable()
    {
        _ammo.Spended += OnAmmoSpended;
        _ammo.Gained += OnAmmoGained;
    }

    private void OnAmmoGained(int value)
    {
        _inventoryItem.TrySetItemCount(value);
    }

    private void OnAmmoSpended(int value)
    {
        _inventoryItem.TrySetItemCount(value);
    }

    private void OnDisable()
    {
        _ammo.Spended -= OnAmmoSpended;
        _ammo.Gained -= OnAmmoGained;
    }
}
