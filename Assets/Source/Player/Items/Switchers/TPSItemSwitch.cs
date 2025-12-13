using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class TPSItemSwitch : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private PlayerAnimator _animator;

    private void OnEnable()
    {
        _animator.Equip(_id);
    }

    // [ServerRpc(RequireOwnership = false)]
    // public void EnableServer()
    // {
    //     EnableObserver();
    // }
    //
    // [ObserversRpc(BufferLast = true)]
    // public void EnableObserver()
    // {
    //     _animator.SetInteger("EquipId", _id);
    //     _animator.SetTrigger("Equip");
    // }
}