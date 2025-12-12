using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class SetActiveObject : NetworkBehaviour
{
    [ServerRpc(RequireOwnership = false)]
    public void SetActiveServer(GameObject gameObject, bool value)
    {
        SetActiveObserver(gameObject, value);
    }

    [ObserversRpc]
    public void SetActiveObserver(GameObject gameObject, bool value)
    {
        gameObject.SetActive(value);
    }
}
