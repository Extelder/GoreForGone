using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorItemSwitcher : MonoBehaviour
{
    [field: SerializeField] public PlayerCharacter Character;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _switchItemTriggerName = "Switch";

    [SerializeField] private GameObject _currentItem;

    private GameObject _prevItem;

    public event Action ItemSwitched;

    public void SwitchItem(GameObject itemToSwitch)
    {
        _prevItem = _currentItem;
        _currentItem.SetActive(false);
        _currentItem = itemToSwitch;
        _animator.SetTrigger(_switchItemTriggerName);
        _currentItem.SetActive(true);
        ItemSwitched?.Invoke();
    }

    public void ReturnToPrevItem()
    {
        SwitchItem(_prevItem);
    }

    public void DisableCurrentItem()
    {
        _currentItem.SetActive(false);
    }
}