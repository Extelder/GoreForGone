using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElectrolizableProjectile : PlayerProjectile
{
    public override void CheckOnComponents(Collider other)
    {
        base.CheckOnComponents(other);
        if (other.TryGetComponent<IElectrifiable>(out IElectrifiable electrifieable))
        {
            electrifieable.InteractElectricity();
        }
    }
}
