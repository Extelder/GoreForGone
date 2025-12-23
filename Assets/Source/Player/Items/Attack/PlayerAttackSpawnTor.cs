using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class PlayerAttackSpawnTor : PlayerAttack
{
    [SerializeField] private GameObject _model;
    [SerializeField] private float _force = 6f;
    [SerializeField] private Transform _spawnEmpty;
    [SerializeField] private PlayVfx _vfx;

    public void SpawnTor()
    {
        if (!base.IsOwner)
            return;
        StartAttack?.Invoke();
        PlayerCharacter.Instance.ServerSpawnObject(_model, _spawnEmpty.position, _spawnEmpty.rotation);
        PlayerCharacter.Instance.PlayerController.AddImpulse(-PlayerCharacter.Instance.Camera.forward, _force);
        _vfx.Play();
        Performed?.Invoke();
    }

    public override event Action Performed;
    public override event Action StartAttack;
}
