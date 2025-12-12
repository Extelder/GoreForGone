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
    public event Action ItemBeginSwitched;

    public virtual void SwitchItem(GameObject itemToSwitch)
    {
        ItemBeginSwitched?.Invoke();
        _prevItem = _currentItem;
        if (_currentItem != null)
            _currentItem.SetActive(false);
        _currentItem = itemToSwitch;
        _animator.SetTrigger(_switchItemTriggerName);
        _currentItem.SetActive(true);
        ItemSwitched?.Invoke();
    }

    public void PlaySwitchAnim()
    {
        _animator.SetTrigger(_switchItemTriggerName);
    }

    public GameObject GetCurrentItem() => _currentItem;

    public void ReturnToPrevItem()
    {
        SwitchItem(_prevItem);
    }

    public void EnableCurrentItem()
    {
        _currentItem.SetActive(true);
    }

    public void DisableCurrentItem()
    {
        _currentItem.SetActive(false);
    }
}