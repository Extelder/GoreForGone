using System;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : NetworkBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _capsuleHeight;
    [SerializeField] private float _capsuleRadius;
    [SerializeField] private float _distance;
    [SerializeField] private LayerMask _capsuleLayerMask;
    [SerializeField] private LayerMask _rayLayerMask;

    private IInteractable _interactable;

    private Vector3 _interactPosition;

    public override void OnStartClient()
    {
        if (!base.IsOwner)
            return;
        PlayerCharacter.Instance.Binds.Character.Interact.performed += TryInteract;
    }

    private void TryInteract(InputAction.CallbackContext obj)
    {
        if (_interactable != null)
        {
            _interactable.Interact();
        }
    }

    private void OnDisable()
    {
        if (!base.IsOwner)
            return;
        PlayerCharacter.Instance.Binds.Character.Interact.performed -= TryInteract;
    }

    private void Update()
    {
        if(!base.IsOwner)
            return;
        Vector3 origin = _camera.transform.position;
        Vector3 direction = _camera.transform.forward;

        Vector3 p1 = origin;
        Vector3 p2 = origin - _camera.transform.up * (_capsuleHeight * 0.5f);

        Debug.DrawRay(origin, direction * _distance, Color.yellow);

        if (Physics.CapsuleCast(p1, p2, _capsuleRadius, direction, out RaycastHit hit, _distance, _capsuleLayerMask))
        {
            if (Physics.Raycast(origin, hit.point - origin, out hit, _distance, _rayLayerMask))
            {
                Debug.DrawRay(origin, (hit.point - origin) * _distance, Color.green);

                if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    if (_interactable != null)
                    {
                        if (_interactable != interactable)
                        {
                            _interactable.Lost();
                        }
                    }

                    _interactable = interactable;
                    _interactable.Detected();
                    if (interactable is MonoBehaviour awawf)
                    {
                        _interactPosition = awawf.transform.position;
                    }

                    return;
                }
            }
        }

        _interactable?.Lost();
        _interactable = null;
        _interactPosition = new Vector3(0, 0, 0);
    }

    // private void OnDrawGizmos()
    // {
    //     
    //     if (_interactable != null)
    //     {
    //         Gizmos.DrawWireSphere(_interactPosition, 0.5f);
    //     }
    //
    //     Vector3 origin = _camera.transform.position;
    //     Vector3 dir = _camera.transform.forward;
    //     Vector3 p1 = origin + _camera.transform.up * (_capsuleHeight * 0.5f);
    //     Vector3 p2 = origin - _camera.transform.up * (_capsuleHeight * 0.5f);
    //
    //     DrawCapsule(p1, p2, _capsuleRadius, Color.black);
    //
    //     DrawCapsule(p1 + dir * _distance, p2 + dir * _distance, _capsuleRadius, new Color(1f, 1f, 0f, 0.3f));
    // }
    //
    // private void DrawCapsule(Vector3 p1, Vector3 p2, float radius, Color color)
    // {
    //     Gizmos.color = color;
    //
    //     Gizmos.DrawLine(p1, p2);
    //
    //     Vector3 up = (p1 - p2).normalized;
    //     Vector3 right = Vector3.Cross(up, Vector3.forward);
    //     if (right == Vector3.zero) right = Vector3.Cross(up, Vector3.up);
    //     right.Normalize();
    //     Vector3 forward = Vector3.Cross(up, right);
    //
    //     DrawCircle(p1, up, radius);
    //     DrawCircle(p2, up, radius);
    //
    //     Gizmos.DrawLine(p1 + right * radius, p2 + right * radius);
    //     Gizmos.DrawLine(p1 - right * radius, p2 - right * radius);
    //     Gizmos.DrawLine(p1 + forward * radius, p2 + forward * radius);
    //     Gizmos.DrawLine(p1 - forward * radius, p2 - forward * radius);
    //
    //     Gizmos.DrawWireSphere(p1, radius);
    //     Gizmos.DrawWireSphere(p2, radius);
    // }
    //
    // private void DrawCircle(Vector3 center, Vector3 normal, float radius)
    // {
    //     const int segments = 16;
    //     Vector3 up = normal.normalized * radius;
    //     Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
    //     Vector3 right = Vector3.Cross(up, forward).normalized * radius;
    //
    //     float angleStep = 360f / segments;
    //     Vector3 prevPoint = center + right;
    //     for (int i = 1; i <= segments; i++)
    //     {
    //         float angle = angleStep * i * Mathf.Deg2Rad;
    //         Vector3 nextPoint = center + (right * Mathf.Cos(angle) + forward * Mathf.Sin(angle));
    //         Gizmos.DrawLine(prevPoint, nextPoint);
    //         prevPoint = nextPoint;
    //     }
    // }
}