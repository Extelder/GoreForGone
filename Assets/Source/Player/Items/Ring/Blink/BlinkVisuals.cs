using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkVisuals : MonoBehaviour
{
    [SerializeField] private Transform _blinkDown;
    [SerializeField] private LayerMask _layerMask;

    private void Update()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 1000f, _layerMask))
        {
            _blinkDown.position = hit.point;
        }
    }
}