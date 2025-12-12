using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryEquipableItem : EquipableItem
{
    protected override void OnEquipActionPerformed(InputAction.CallbackContext obj)
    {
        if (itemObject.activeInHierarchy)
        {
            BothHandsAnimatorItemSwitcher bothHandsAnimatorItemSwitcher = (BothHandsAnimatorItemSwitcher) _itemSwitcher;
            bothHandsAnimatorItemSwitcher.ReturnToPreviousItems();
            return;
        }

        base.OnEquipActionPerformed(obj);
    }
}