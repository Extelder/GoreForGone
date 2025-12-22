using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSpawnProjectile : PlayerAttack
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _spawnOrigin;
    
    public override event Action Performed;
    public override event Action StartAttack;

    public void SpawnProjectile()
    {
        if (!base.IsOwner)
            return;
        StartAttack?.Invoke();
        PlayerCharacter.Instance.ServerSpawnObject(_projectile, _spawnOrigin.position, _spawnOrigin.rotation);
        Performed?.Invoke();
    }
}
