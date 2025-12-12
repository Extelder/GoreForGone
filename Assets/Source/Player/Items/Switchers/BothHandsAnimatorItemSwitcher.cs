using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BothHandsAnimatorItemSwitcher : AnimatorItemSwitcher
{
    [SerializeField] private AnimatorItemSwitcher[] _otherHandsSwitchers;

    private void OnEnable()
    {
        for (int i = 0; i < _otherHandsSwitchers.Length; i++)
        {
            _otherHandsSwitchers[i].ItemSwitched += OnItemSwitched;
        }
    }

    public override void SwitchItem(GameObject itemToSwitch)
    {
        for (int i = 0; i < _otherHandsSwitchers.Length; i++)
        {
            _otherHandsSwitchers[i].DisableCurrentItem();
        }

        base.SwitchItem(itemToSwitch);
    }

    private void OnItemSwitched()
    {
        if (GetCurrentItem().activeInHierarchy == false)
            return;
        DisableCurrentItem();
        for (int i = 0; i < _otherHandsSwitchers.Length; i++)
        {
            _otherHandsSwitchers[i].EnableCurrentItem();
            _otherHandsSwitchers[i].PlaySwitchAnim();
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _otherHandsSwitchers.Length; i++)
        {
            _otherHandsSwitchers[i].ItemSwitched -= OnItemSwitched;
        }
    }
}