using System;
using System.Collections;
using System.Collections.Generic;
using MilkShake;
using UnityEngine;

[Serializable]
public class AttackShake
{
    [field: SerializeField] public PlayerAttack Attack { get; private set; }
    [field: SerializeField] public ShakePreset Preset { get; private set; }

    public void Subscribe()
    {
        Attack.Performed += OnAttackPerformed;
    }

    private void OnAttackPerformed()
    {
        PlayerCharacter.Instance.Shaker.Shake(Preset);
    }

    public void Describe()
    {
        Attack.Performed -= OnAttackPerformed;
    }
}

public class PlayerAttackShake : MonoBehaviour
{
    [SerializeField] private AttackShake[] _attackShakes;

    private void OnEnable()
    {
        for (int i = 0; i < _attackShakes.Length; i++)
        {
            _attackShakes[i].Subscribe();
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _attackShakes.Length; i++)
        {
            _attackShakes[i].Describe();
        }
    }
}