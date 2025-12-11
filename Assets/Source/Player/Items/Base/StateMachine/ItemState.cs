using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemState : State
{
    [field: SerializeField] public ItemAnimator Animator { get; private set; }

    public abstract override void Enter();
}