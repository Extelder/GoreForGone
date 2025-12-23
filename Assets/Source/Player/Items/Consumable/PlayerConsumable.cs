using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConsumable : MonoBehaviour
{
    [SerializeField] private EquipableItem _equipableItem;
    [SerializeField] private UseInventoryItem1 _inventoryItem;

    public virtual void Consume()
    {
        _inventoryItem.TrySpendItem();
        if (_inventoryItem.Count == 0) _equipableItem.Lock();
    }
}