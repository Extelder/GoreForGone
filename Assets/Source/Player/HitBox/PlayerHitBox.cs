using System;
using System.Collections;
using FishNet.Object;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerHitBox : NetworkBehaviour
{
    [field: SerializeField] public float DamageMultiplier { get; set; } = 1;
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private float _notActiveDelayAfterSpawn;
    public event Action<bool, EnemyStateMachine> TryParryAttack;
    private bool _active;

    public override void OnStartClient()
    {
        StopAllCoroutines();
        StartCoroutine(WaitForDelay());
    }


    private IEnumerator WaitForDelay()
    {
        yield return new WaitForSeconds(_notActiveDelayAfterSpawn);
        _active = true;
    }

    public void TryParry(bool canParry, EnemyStateMachine parriableEnemy)
    {
        TryParryAttack?.Invoke(canParry, parriableEnemy);   
    }

    [ServerRpc(RequireOwnership = false)]
    public virtual void TakeDamage(float damage)
    {
        if (!_active)
            return;
        damage *= DamageMultiplier;
        TakeDamageObserver(damage, _health);
    }

    [ObserversRpc]
    public void TakeDamageObserver(float damage, PlayerHealth health)
    {
        health.TakeDamage(damage);
    }
}