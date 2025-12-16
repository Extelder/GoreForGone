using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class BlinkVisuals : MonoBehaviour
{
    [SerializeField] private Collider _fullBodyCheck;
    [SerializeField] private Collider _crouchBodyCheck;

    [SerializeField] private Transform _blinkDown;
    [SerializeField] private LayerMask _layerMask;

    private CompositeDisposable _disposable = new CompositeDisposable();

    //
    // private void OnEnable()
    // {
    //     _fullBodyCheck.OnTriggerEnterAsObservable().Subscribe(_ =>
    //     {
    //         
    //     }).AddTo(_disposable);
    //     _fullBodyCheck.OnTriggerExitAsObservable().Subscribe(_ => { }).AddTo(_disposable);
    // }
    //
    // private void OnDisable()
    // {
    //     _disposable?.Clear();
    // }

    private void Update()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 1000f, _layerMask))
        {
            _blinkDown.position = hit.point;
        }
    }
}