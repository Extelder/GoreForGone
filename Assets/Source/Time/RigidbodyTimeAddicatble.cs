using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyTimeAddicatble : TimeAddictableObject
{
    [SerializeField] private Rigidbody _rigidbody;

    protected override void OnTimeValueChanged(float value)
    {
        _rigidbody.velocity *= value;
    }
}