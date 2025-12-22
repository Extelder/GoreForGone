using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectrifiableItemAnimatable : ElectrifiableItem
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _boolName;
    public override void InteractElectricity()
    {
        base.InteractElectricity();
        _animator.SetBool(_boolName, true);
    }
}
