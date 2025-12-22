using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public abstract class PlayerAttack : NetworkBehaviour
{
    public abstract event Action Performed;
    public abstract event Action StartAttack;
}