using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    public override void OnCollisionEnterVirtual(Collision other)
    {
        if (other.collider == PlayerCharacter.Instance.PlayerCollider)
            return;
        base.OnCollisionEnterVirtual(other);
    }

    public override void OnTriggerEnterVirtual(Collider other)
    {
        if (other == PlayerCharacter.Instance.PlayerCollider)
            return;
        base.OnTriggerEnterVirtual(other);
    }
}
