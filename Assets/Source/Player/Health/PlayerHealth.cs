using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public override void Death()
    {
        CurrentValue = 0;
        SetCurrentValue(CurrentValue);
    }
}