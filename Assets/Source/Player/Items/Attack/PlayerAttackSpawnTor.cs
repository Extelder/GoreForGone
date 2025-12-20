using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class PlayerAttackSpawnTor : NetworkBehaviour
{
    [SerializeField] private PlayerAttack _playerAttack;
    [SerializeField] private GameObject _model;
    [SerializeField] private float _force = 6f;
    [SerializeField] private GameObject _SpawnEmpty;

    public override void OnStartClient()
    {
        _playerAttack.Performed += OnPerformed;
    }

    private void OnDisable()
    {
        _playerAttack.Performed -= OnPerformed;
    }

    private void OnPerformed()
    {
        if (!base.IsOwner) return;
        PlayerCharacter.Instance.ServerSpawnObject(_model, _SpawnEmpty.transform.position, _SpawnEmpty.transform.rotation);
        PlayerCharacter.Instance.PlayerController.AddImpulse(-PlayerCharacter.Instance.Camera.forward, _force);
    }
}
