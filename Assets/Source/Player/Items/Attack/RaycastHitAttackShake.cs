using System;
using System.Collections;
using System.Collections.Generic;
using MilkShake;
using UnityEngine;

public class RaycastHitAttackShake : MonoBehaviour
{
    [SerializeField] private ShakePreset _shakePreset;
    [SerializeField] private RaycastAttack _raycastAttack;

    private void OnEnable()
    {
        _raycastAttack.Hitted += OnHitted;
    }

    private void OnHitted()
    {
        PlayerCharacter.Instance.Shaker.Shake(_shakePreset);
    }

    private void OnDisable()
    {
        _raycastAttack.Hitted -= OnHitted;
    }
}