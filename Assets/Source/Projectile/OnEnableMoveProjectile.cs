using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class OnEnableMoveProjectile : MonoBehaviour
{
    [SerializeField] private Transform _projectile;
    [SerializeField] private float _projectileSpeed;

    private CompositeDisposable _disposable = new CompositeDisposable();

    private void OnEnable()
    {
        Move();
    }

    private void Move()
    {
        _disposable.Clear();
        Observable.EveryUpdate().Subscribe(_ =>
        {
            _projectile.position = Vector3.MoveTowards(_projectile.position,
                _projectile.position + _projectile.forward, _projectileSpeed);
        }).AddTo(_disposable);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}