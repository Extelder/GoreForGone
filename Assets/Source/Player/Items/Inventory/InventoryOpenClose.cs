using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOpenClose : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private string _isOpenBoolName = "IsOpen";
    private bool _isOpen;

    private void OnMouseDown()
    {
        _isOpen = !_isOpen;
        _animator.SetBool(_isOpenBoolName, _isOpen);
    }
}