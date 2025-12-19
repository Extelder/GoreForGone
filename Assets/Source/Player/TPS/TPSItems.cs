using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class TPSItems : NetworkBehaviour
{
    [SerializeField] private GameObject[] _items;

    public override void OnStartClient()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            PlayerCharacter.Instance.SetObjectEnableServer(_items[i], false);
        }
    }
}