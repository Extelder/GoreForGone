using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Transform _hitTargetPoint;
    [SerializeField] private GameObject _hint;
    [field: SerializeField] public ItemData ItemData { get; private set; }

    [SerializeField] private InventoryItemSelector[] _items;

    private int _count = 0;

    public void AddItem()
    {
        if (_count + 1 > _items.Length)
        {
            Drop();
            return;
        }

        _count++;
        for (int i = 0; i < _count; i++)
        {
            _items[i].gameObject.SetActive(true);
        }
    }

    public void Select()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i].Outline.enabled = true;
        }

        _hint.transform.position = _hitTargetPoint.position;
        _hint.transform.LookAt(PlayerCharacter.Instance.Camera);
        _hint.SetActive(true);
        PlayerCharacter.Instance.Binds.Character.Drop.performed += OnDropPerformed;
    }

    private void OnDropPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("DROp");
        Drop();
        _items[_count - 1].gameObject.SetActive(false);
        _count--;
    }

    public void Drop()
    {
        PlayerCharacter.Instance.PlayerDrop.DropItem(ItemData);
    }

    public void DeSelect()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i].Outline.enabled = false;
        }

        _hint.SetActive(false);
        _hint.transform.position = new Vector3(-1000, 10000, 10000);
        PlayerCharacter.Instance.Binds.Character.Drop.performed -= OnDropPerformed;
    }

    private void OnDisable()
    {
        DeSelect();
    }
}