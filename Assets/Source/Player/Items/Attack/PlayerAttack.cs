using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : MonoBehaviour
{
    public abstract event Action Performed;
    public abstract event Action StartAttack;
}