using System;
using System.Collections.Generic;
using FishNet.Object;
using UniRx;
using UnityEngine;

public class MeleeTracer : NetworkBehaviour
{
    [field: SerializeField] public float Damage { get; private set; }

    [Range(2, 12)] [SerializeField] private int _segments = 6;

    [SerializeField] private LayerMask _hitMask;
    [SerializeField] private QueryTriggerInteraction _triggerInteraction = QueryTriggerInteraction.Ignore;

    [SerializeField] private GameObject _testHit;
    [SerializeField] private Transform _bladeBase;
    [SerializeField] private Transform _bladeTip;
    [SerializeField] private float _radius = 0.08f;
    
    private Vector3[] _previousPoints;
    private RaycastHit[] _hits;
    private Collider[] _overlapColliders;

    private Collider _sphereCastCollider;

    private CompositeDisposable _disposable = new CompositeDisposable();

    readonly HashSet<Collider> _hitThisSwing = new HashSet<Collider>();

    public override void OnStartClient()
    {
        base.OnStartClient();
        _previousPoints = new Vector3[Mathf.Max(_segments, 2)];
    }

    public void BeginSwing()
    {
        _hitThisSwing.Clear();
        Swing();
        CacheCurrentPoints(_previousPoints);
    }

    public void EndSwing()
    {
        _disposable.Clear();
    }

    private void Swing()
    {
        Observable.Interval(TimeSpan.FromSeconds(0.02f)).Subscribe(_ =>
        {
            Vector3[] currentPoints = GetCurrentPointsTemp();

            for (int i = 0; i < currentPoints.Length; i++)
            {
                Vector3 delta = currentPoints[i] - _previousPoints[i];
                float distance = delta.magnitude;

                if (distance > 0.0005f)
                {
                    Vector3 direction = delta / distance;
                    _hits = Physics.SphereCastAll(_previousPoints[i], _radius, direction, distance, _hitMask,
                        _triggerInteraction);

                    foreach (var hit in _hits)
                    {
                        var _sphereCastCollider = hit.collider;
                        if (_sphereCastCollider == null) continue;
                        if (_hitThisSwing.Contains(_sphereCastCollider)) continue;

                        _hitThisSwing.Add(_sphereCastCollider);

                        //МЕТОЧКА ДЛЯ МЕНЯ - ВЫНЕСТИ В ХИТБОКС ОТДЕЛЬНО ЭТУ ХУЕТУ, ЕСЛИ ТЫ ЭТО ЧИТАЕШЬ, ТО ОТСОСИ САМОМУ СЕБЕ
                        Instantiate(_testHit, hit.point, Quaternion.identity);
                        if (_sphereCastCollider.TryGetComponent<IWeaponVisitor>(out IWeaponVisitor visitor))
                            visitor.Visit(this);
                    }
                }
                else
                {
                    _overlapColliders =
                        Physics.OverlapSphere(currentPoints[i], _radius, _hitMask, _triggerInteraction);
                    foreach (var collider in _overlapColliders)
                    {
                        if (_hitThisSwing.Contains(collider)) continue;

                        _hitThisSwing.Add(collider);

                        Instantiate(_testHit, currentPoints[i], Quaternion.identity);
                        if (collider.TryGetComponent<IWeaponVisitor>(out IWeaponVisitor visitor))
                            visitor.Visit(this);
                    }
                }
            }

            for (int i = 0; i < _previousPoints.Length; i++)
                _previousPoints[i] = currentPoints[i];
        }).AddTo(_disposable);
    }

    private Vector3[] GetCurrentPointsTemp()
    {
        var pointsAlongSword = new Vector3[Mathf.Max(_segments, 2)];
        CacheCurrentPoints(pointsAlongSword);
        return pointsAlongSword;
    }

    private void CacheCurrentPoints(Vector3[] pointAlongSword)
    {
        for (int i = 0; i < pointAlongSword.Length; i++)
        {
            float pointsDistribution = (pointAlongSword.Length == 1) ? 0f : (float) i / (pointAlongSword.Length - 1);
            pointAlongSword[i] = Vector3.Lerp(_bladeBase.position, _bladeTip.position, pointsDistribution);
        }
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}