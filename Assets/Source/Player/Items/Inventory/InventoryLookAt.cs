using System;
using UnityEngine;

public class InventoryLookAt : MonoBehaviour
{
    [SerializeField] private GameObject _targetObject;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField, Range(0.1f, 20f)] private float rotationSpeed = 5f;
    [SerializeField] private bool ignoreY = false;
    [SerializeField] private Vector3 _lookOffset = new Vector3(0f, 1f, 0f);
    [SerializeField, Range(0f, 90f)] private float verticalLimit = 60f;
    [SerializeField, Range(-90f, 90f)] private float zRotationOffset = 0f;

    private Vector3 targetPoint;

    private void LateUpdate()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 10f, _layerMask);

        foreach (var hit in hits)
        {
            if (hit.collider == null) continue;
            if (hit.collider.gameObject == _targetObject)
            {
                targetPoint = hit.point;
                if (ignoreY)
                    targetPoint.y = transform.position.y;
                break;
            }
        }

        Vector3 direction = (targetPoint - transform.position + _lookOffset).normalized;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            Vector3 euler = targetRotation.eulerAngles;
            if (euler.x > 180f) euler.x -= 360f;
            euler.x = Mathf.Clamp(euler.x, -verticalLimit, verticalLimit);
            euler.z += zRotationOffset;
            targetRotation = Quaternion.Euler(euler);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}