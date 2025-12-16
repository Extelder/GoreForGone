using System;
using FishNet.Object;
using UniRx;
using UnityEngine;

public class LookAtClosestPlayerNotIK : NetworkBehaviour
{
    [SerializeField] private float _checkRate;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private Transform _lookAtBone;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private bool _startOnEnable;
    [SerializeField] private float _maxLookAngle = 100f;

    private CompositeDisposable _disposable = new CompositeDisposable();

    public override void OnStartClient()
    {
        if (!IsServer)
            return;
        base.OnStartClient();
        if (_startOnEnable)
        {
            Debug.Log("LOOK AT");
            StartLookAt();
        }
    }

    public void StartLookAt()
    {
        Observable.Interval(TimeSpan.FromSeconds(_checkRate))
            .Subscribe(_ =>
            {
                PlayerCharacter nearestCharacter =
                    FindNearestPlayerCharacter(_lookAtBone.position);

                if (nearestCharacter == null)
                    return;

                Vector3 direction =
                    nearestCharacter.LookAtPoint.position - _lookAtBone.position;

                direction.y = 0f;

                if (direction.sqrMagnitude < 0.001f)
                    return;

                Vector3 currentForward = _lookAtBone.forward;
                currentForward.y = 0f;

                float angle = Vector3.SignedAngle(
                    currentForward,
                    direction,
                    Vector3.up
                );

                angle = Mathf.Clamp(angle, -_maxLookAngle, _maxLookAngle);

                Vector3 clampedDir =
                    Quaternion.AngleAxis(angle, Vector3.up) * currentForward;

                Quaternion lookRot = Quaternion.LookRotation(clampedDir);
                lookRot *= Quaternion.Euler(_offset);

                _lookAtBone.rotation = Quaternion.Slerp(
                    _lookAtBone.rotation,
                    lookRot,
                    _turnSpeed * Time.deltaTime
                );
            })
            .AddTo(_disposable);
    }

    public void StopLookAt()
    {
        _disposable.Clear();
    }

    private PlayerCharacter FindNearestPlayerCharacter(Vector3 fromPosition)
    {
        PlayerCharacter[] characters =
            PlayerCharacter.Instance.Characters.ToArray();

        PlayerCharacter nearest = null;
        float minDistSq = float.MaxValue;

        foreach (var character in characters)
        {
            if (character == null)
                continue;

            float distSq =
                (character.PlayerTransform.position - fromPosition).sqrMagnitude;

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
        if (!IsServer)
            return;

        _disposable.Clear();
    }
}