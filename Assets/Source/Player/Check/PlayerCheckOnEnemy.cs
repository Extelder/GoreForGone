using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerCheckOnEnemy : MonoBehaviour
{
    [SerializeField] private RaycastSettings _raycastSettings;
    
    public bool EnemyDetected()
    {
        bool hitted = Physics.Raycast(_raycastSettings.Origin.position, _raycastSettings.Origin.forward, out RaycastHit hit,
            _raycastSettings.MaxDistance, _raycastSettings.LayerMask);
        Debug.DrawRay(_raycastSettings.Origin.position, Vector3.forward * _raycastSettings.MaxDistance);
        if (hitted)
        {
            if (hit.collider.TryGetComponent<EnemyHear>(out EnemyHear hear))
            {
                return true;
            }
        }
        return false;
    }
}