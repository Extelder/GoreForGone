using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [field: SerializeField] public ItemData ItemData { get; private set; }

    [SerializeField] private GameObject[] _items;

    private int _count = 0;

    public void AddItem()
    {
        _count++;
        for (int i = 0; i < _count; i++)
        {
            _items[i].SetActive(true);
        }
    }
}