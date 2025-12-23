using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerDamageableAttack : PlayerAttack
{
    [field: SerializeField] public float Damage { get; private set; }
    public abstract override event Action Performed;
    public abstract override event Action StartAttack;
}
