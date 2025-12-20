using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private InventoryItem[] _inventoryItems;

    public void EquipItem(ItemData itemData)
    {
        for (int i = 0; i < _inventoryItems.Length; i++)
        {
            if (_inventoryItems[i].ItemData == itemData)
            {
                _inventoryItems[i].AddItem();
                return;
            }
        }
    }
}