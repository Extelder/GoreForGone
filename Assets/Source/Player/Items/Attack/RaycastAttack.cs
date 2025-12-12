using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastAttack : PlayerAttack
{
    [SerializeField] private RaycastSettings _raycastSettings;

    public override event Action Performed;
    public override event Action StartAttack;

    public event Action Hitted;

    private RaycastHit _hit;

    public bool HittedNonDamagableObject()
    {
        if (Physics.Raycast(_raycastSettings.Origin.position, _raycastSettings.Origin.forward, out _hit,
            _raycastSettings.MaxDistance, _raycastSettings.LayerMask))
        {
            return true;
        }

        return false;
    }

    public void PerformRaycastAttack()
    {
        StartAttack?.Invoke();
        Performed?.Invoke();

        if (Physics.Raycast(_raycastSettings.Origin.position, _raycastSettings.Origin.forward, out _hit,
            _raycastSettings.MaxDistance, _raycastSettings.LayerMask))
        {
            Hitted?.Invoke();
        }
    }
}