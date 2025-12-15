using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UniRx;
using UnityEngine;

public class LookAtClosestPlayerNotIK : NetworkBehaviour
{
    [SerializeField] private float _checkRate;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private Transform _lookAtBone;
    private CompositeDisposable _disposable = new CompositeDisposable();

    public void StartLookAt()
    {
        Observable.Interval(TimeSpan.FromSeconds(_checkRate)).Subscribe(_ =>
        {
            PlayerCharacter nearestCharacter = FindNearestPlayerCharacter(_lookAtBone.position);
            if (nearestCharacter == null)
                return;
            Vector3 direction = nearestCharacter.LookAtPoint.position - _lookAtBone.position;
            direction.y = 0;
            _lookAtBone.rotation = Quaternion.Slerp(
                _lookAtBone.rotation,
                Quaternion.LookRotation(direction),
                _turnSpeed * Time.deltaTime);
        }).AddTo(_disposable);
    }

    public void StopLookAt()
    {
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