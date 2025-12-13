using System;
using System.Collections;
using FishNet.Object;
using UnityEngine;

public class UnitPlayerDetector : NetworkBehaviour
{
    [Header("Vision Settings")]
    [SerializeField] private int _horizontalRays = 25;
    [SerializeField] private int _verticalRays = 15;
    [SerializeField] private float _horizontalFov = 100f;
    [SerializeField] private float _verticalFov = 60f;
    [SerializeField] private float _viewDistance = 20f;
    [SerializeField] private LayerMask _visionMask;
    [SerializeField] private Transform _eyesPoint;

    [Header("Detection Timings")]
    [SerializeField] private float _inspectTime = 0.2f;
    [SerializeField] private float _chaseTime = 0.3f;
    [SerializeField] private float _timeToUndetect = 0.6f;

    [Header("AI")]
    [SerializeField] private EnemyStateMachine _stateMachine;

    public event Action PlayerDetected;
    public event Action PlayerLost;

    private Transform _player;
    private PlayerCharacter _character;

    private bool _detected;
    private bool _playerVisibleThisTick;
    private bool _isSeeingPlayer;

    private float _seeTimer;
    private float _timeSinceLastSeen;

    private Vector3[,] _rayGrid;
    private bool _gridDirty = true;
    private RaycastHit _hit;

    private void OnValidate()
    {
        _horizontalRays = Mathf.Max(2, _horizontalRays);
        _verticalRays = Mathf.Max(2, _verticalRays);
        _horizontalFov = Mathf.Clamp(_horizontalFov, 1f, 360f);
        _verticalFov = Mathf.Clamp(_verticalFov, 1f, 180f);
        _viewDistance = Mathf.Max(0.1f, _viewDistance);

        _inspectTime = Mathf.Max(0f, _inspectTime);
        _chaseTime = Mathf.Max(_inspectTime, _chaseTime);

        _gridDirty = true;

        if (!Application.isPlaying)
            GenerateRayGrid();
    }

    public override void OnStartClient()
    {
        if (!IsServer)
            return;

        TryFindPlayer();

        if (_gridDirty || _rayGrid == null)
            GenerateRayGrid();

        StartCoroutine(VisionLoop());
    }


    private IEnumerator VisionLoop()
    {
        WaitForSeconds delay = new WaitForSeconds(0.05f);

        while (true)
        {
            if (_rayGrid == null || _gridDirty)
                GenerateRayGrid();

            if (_player == null)
                TryFindPlayer();

            _playerVisibleThisTick = false;

            if (_player != null)
                CastVisionRays();

            HandleDetectionState();

            yield return delay;
        }
    }

    private void CastVisionRays()
    {
        Vector3 origin = _eyesPoint
            ? _eyesPoint.position
            : transform.position + Vector3.up * 1.6f;

        for (int y = 0; y < _verticalRays; y++)
        {
            for (int x = 0; x < _horizontalRays; x++)
            {
                Vector3 dir = transform.rotation * _rayGrid[x, y];

                if (Physics.Raycast(origin, dir, out _hit, _viewDistance, _visionMask))
                {
                    if (_hit.collider.TryGetComponent(out PlayerCharacter pc))
                    {
                        _character = pc;
                        _playerVisibleThisTick = true;

                        Debug.DrawRay(origin, dir * _hit.distance, Color.red, 0.05f);
                        return; // 🔥 нашли — дальше не ебём сцену
                    }

                    Debug.DrawRay(origin, dir * _hit.distance, Color.gray, 0.05f);
                }
                else
                {
                    Debug.DrawRay(origin, dir * _viewDistance, Color.green, 0.05f);
                }
            }
        }
    }

    private void HandleDetectionState()
    {
        if (_playerVisibleThisTick)
        {
            _timeSinceLastSeen = 0f;

            if (!_detected)
            {
                _detected = true;
                PlayerDetected?.Invoke();
            }

            UpdateAI(true);
        }
        else
        {
            _timeSinceLastSeen += 0.05f;

            if (_detected && _timeSinceLastSeen >= _timeToUndetect)
            {
                _detected = false;
                PlayerLost?.Invoke();
            }

            UpdateAI(false);
        }
    }


    private void UpdateAI(bool playerVisible)
    {
        if (!playerVisible)
        {
            _seeTimer = 0f;
            _isSeeingPlayer = false;
            return;
        }

        _seeTimer += 0.05f;

        if (!_isSeeingPlayer && _seeTimer >= _inspectTime)
        {
            _isSeeingPlayer = true;
            _stateMachine?.Inspect(_character.PlayerTransform.position);
        }

        if (_seeTimer >= _chaseTime)
        {
            _stateMachine?.Chase(_character.PlayerTransform);
        }
    }


    private void TryFindPlayer()
    {
        var pc = FindObjectOfType<PlayerCharacter>();
        if (pc != null)
            _player = pc.PlayerTransform;
    }

    private void GenerateRayGrid()
    {
        _rayGrid = new Vector3[_horizontalRays, _verticalRays];

        float hx = Mathf.Max(1, _horizontalRays - 1);
        float vy = Mathf.Max(1, _verticalRays - 1);

        for (int y = 0; y < _verticalRays; y++)
        {
            for (int x = 0; x < _horizontalRays; x++)
            {
                float tX = x / hx;
                float tY = y / vy;

                float yaw = Mathf.Lerp(-_horizontalFov * 0.5f, _horizontalFov * 0.5f, tX);
                float pitch = Mathf.Lerp(-_verticalFov * 0.5f, _verticalFov * 0.5f, tY);

                _rayGrid[x, y] = Quaternion.Euler(-pitch, yaw, 0f) * Vector3.forward;
            }
        }

        _gridDirty = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (_rayGrid == null || _gridDirty)
            GenerateRayGrid();

        if (_rayGrid == null)
            return;

        Vector3 origin = _eyesPoint
            ? _eyesPoint.position
            : transform.position + Vector3.up * 1.6f;

        Gizmos.color = new Color(0f, 0.8f, 1f, 0.7f);

        for (int y = 0; y < _verticalRays; y++)
        {
            for (int x = 0; x < _horizontalRays; x++)
            {
                Vector3 dir = transform.rotation * _rayGrid[x, y];
                Gizmos.DrawRay(origin, dir * _viewDistance * 0.4f);
            }
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, 0.06f);
    }

    public void MarkGridDirty() => _gridDirty = true;
}
