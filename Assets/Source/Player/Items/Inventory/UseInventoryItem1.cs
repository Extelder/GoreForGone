using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseInventoryItem1 : InventoryItem
{
    [SerializeField] private EquipableItem _equipableItem;

    public override void Select()
    {
        base.Select();
        PlayerCharacter.Instance.Binds.Character.MainShoot.performed += OnMainActionPerformed;
        Debug.Log("Selected");
    }

    private void OnMainActionPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("Unlock");
        _equipableItem.Unlock();
        _equipableItem.Equip();
    }

    public override void DeSelect()
    {
        base.DeSelect();
        PlayerCharacter.Instance.Binds.Character.MainShoot.performed -= OnMainActionPerformed;
        Debug.Log("DeSelected");
    }
}