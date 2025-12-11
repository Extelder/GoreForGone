using System;
using System.Collections;
using System.Collections.Generic;
using MilkShake;
using UnityEngine;

public class PlayerAttackShake : MonoBehaviour
{
    [SerializeField] private ShakePreset _shakePreset;
    [SerializeField] private PlayerAttack _attack;

    private void OnEnable()
    {
        _attack.Performed += OnAttackPerformed;
    }

    private void OnAttackPerformed()
    {
        PlayerCharacter.Instance.Shaker.Shake(_shakePreset);
    }

    private void OnDisable()
    {
        _attack.Performed -= OnAttackPerformed;
    }
}