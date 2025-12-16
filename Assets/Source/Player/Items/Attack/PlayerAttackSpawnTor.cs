using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSpawnTor : MonoBehaviour
{
    [SerializeField] private PlayerAttack _playerAttack;
    [SerializeField] private GameObject _model;
    [SerializeField] private GameObject _SpawnEmpty;

    private void OnEnable()
    {
        _playerAttack.Performed += OnPerformed;
    }
    private void OnDisable()
    {
        _playerAttack.Performed -= OnPerformed;
    }

    private void OnPerformed()
    {
        PlayerCharacter.Instance.ServerSpawnObject(_model, _SpawnEmpty.transform.position, _SpawnEmpty.transform.rotation);
    }
}
