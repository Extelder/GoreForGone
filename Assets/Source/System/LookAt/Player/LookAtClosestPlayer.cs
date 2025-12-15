using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UniRx;
using UnityEngine;

public class LookAtClosestPlayer : NetworkBehaviour
{
    [SerializeField] private float _lookAtUpdateRate;
    private Transform _startPoint;

    private CompositeDisposable _disposable = new CompositeDisposable();
    public override void OnStartClient()
    {
        if (!base.IsServer)
            return;
        base.OnStartClient();
        _startPoint = transform;
    }

    public void StartLookAt()
    {
        _disposable.Clear();
        Observable.Interval(TimeSpan.FromSeconds(_lookAtUpdateRate)).Subscribe(_ =>
        {
            PlayerCharacter nearestCharacter = FindNearestPlayerCharacter(transform.position);
            if (nearestCharacter != null)
                transform.position = nearestCharacter.LookAtPoint.position;
        }).AddTo(_disposable);
    }

    public void StopLookAt()
    {
        transform.position = _startPoint.position;
        _disposable.Clear();
    }

    private PlayerCharacter FindNearestPlayerCharacter(Vector3 fromPosition)
    {
        PlayerCharacter[] characters = PlayerCharacter.Instance.Characters.ToArray();
        PlayerCharacter nearest = null;
        float minDistSq = float.MaxValue;

        foreach (var character in characters)
        {
            if (character == null) continue;

            float distSq = (character.PlayerTransform.position - fromPosition).sqrMagnitude;
            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                nearest = character;
            }
        }

        return nearest;
    }

    private void OnDisable()
    {
        if (!base.IsServer)
            return;
        _disposable.Clear();
    }
}
