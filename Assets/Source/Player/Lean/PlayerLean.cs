using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerLean : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _character;
    [SerializeField] private Transform _camera;

    [SerializeField] private float _maxAngle = 12f;
    [SerializeField] private float _positionOffset = 0.25f;
    [SerializeField] private float _smooth = 10f;

    private CompositeDisposable _disposable = new CompositeDisposable();
    private PlayerBinds _binds;

    private float _currentLean;
    private float _targetLean;

    private Vector3 _startLocalPos;
    private Quaternion _startLocalRot;

    private void Awake()
    {
        _character.ClientStarted += OnClientStarted;
    }

    private void OnDisable()
    {
        _disposable?.Clear();
        _character.ClientStarted -= OnClientStarted;
    }

    private void OnClientStarted()
    {
        _binds = _character.Binds;

        _startLocalPos = _camera.localPosition;
        _startLocalRot = _camera.localRotation;

        Observable.EveryUpdate()
            .Subscribe(_ => UpdateLean())
            .AddTo(_disposable);
    }

    private void UpdateLean()
    {
        float input = _binds.Character.Lean.ReadValue<float>(); // -1 .. 1

        _targetLean = GetWallLimitedLean(input);

        _currentLean = Mathf.Lerp(
            _currentLean,
            _targetLean,
            Time.deltaTime * _smooth
        );

        float angle = _currentLean * _maxAngle;
        _camera.localRotation =
            _startLocalRot * Quaternion.Euler(0, 0, -angle);

        Vector3 offset =
            Vector3.right * _currentLean * _positionOffset;

        _camera.localPosition =
            Vector3.Lerp(
                _camera.localPosition,
                _startLocalPos + offset,
                Time.deltaTime * _smooth
            );
    }

    private float GetWallLimitedLean(float targetLean)
    {
        if (Mathf.Approximately(targetLean, 0f))
            return 0f;

        Vector3 dir = _camera.right * Mathf.Sign(targetLean);

        float maxDistance = _positionOffset;

        if (Physics.Raycast(
            _camera.position,
            dir,
            out RaycastHit hit,
            maxDistance))
        {
            float allowed =
                Mathf.Clamp(hit.distance - 0.05f, 0f, maxDistance);

            return Mathf.Sign(targetLean) * (allowed / maxDistance);
        }

        return targetLean;
    }
}