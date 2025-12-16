using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordChargeReadyVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _chargeReadyVFX;

    [SerializeField] private SwordChargeAttackState _swordChargeAttackState;

    private void OnEnable()
    {
        _swordChargeAttackState.Charged += OnCharged;
    }

    private void OnCharged()
    {
        _chargeReadyVFX.Play();
    }

    private void OnDisable()
    {
        _swordChargeAttackState.Charged -= OnCharged;
    }
}