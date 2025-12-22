using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrop : MonoBehaviour
{
    [SerializeField] private float _offset;

    [SerializeField] private RaycastSettings _dropSettings;
    private Vector3 _targetPosition;


    public void DropItem(ItemData item)
    {
        if (Physics.Raycast(_dropSettings.Origin.position, _dropSettings.Origin.forward, out RaycastHit hit,
            _dropSettings.MaxDistance, _dropSettings.LayerMask))
        {
            _targetPosition = hit.point + hit.normal * _offset;
        }
        else
        {
            _targetPosition = _dropSettings.Origin.position +
                              _dropSettings.Origin.forward * _dropSettings.MaxDistance;
        }

        PlayerCharacter.Instance.ServerSpawnObject(item.Prefab, _targetPosition, Quaternion.identity);
    }
}