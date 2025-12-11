using System;
using System.Collections;
using System.Collections.Generic;
using EvolveGames;
using FishNet.Object;
using MilkShake;
using UnityEngine;

public class PlayerCharacter : NetworkBehaviour
{
    [field: SerializeField] public PlayerController PlayerController { get; private set; }
    [field: SerializeField] public List<PlayerCharacter> Characters { get; private set; }
    [field: SerializeField] public Transform DropPoint { get; private set; }
    [field: SerializeField] public PlayerBinds Binds;
    [field: SerializeField] public Transform PlayerTransform;
    [field: SerializeField] public Transform Camera;
    [field: SerializeField] public Shaker Shaker { get; private set; }
    [field: SerializeField] public GameObject[] _thirdPerson;

    public static PlayerCharacter Instance { get; private set; }

    public event Action ClientStarted;

    [ServerRpc(RequireOwnership = false)]
    public void ServerSpawnObject(GameObject spawnedObject, Vector3 position, Quaternion rotation)
    {
        GameObject instance = Instantiate(spawnedObject, position, rotation);

        ServerManager.Spawn(spawnedObject);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            Binds = InputManager.inputActions;

            Binds.Enable();

            for (int i = 0; i < _thirdPerson.Length; i++)
            {
                _thirdPerson[i].SetActive(false);
            }

            Instance = this;
        }

        ClientStarted?.Invoke();
    }

    public override void OnStopClient()
    {
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetObjectEnableServer(GameObject needObject, bool enabled)
    {
        SetObjectEnableObserver(needObject, enabled);
    }

    [ObserversRpc]
    public void SetObjectEnableObserver(GameObject gameObject, bool enabled)
    {
        gameObject.SetActive(enabled);
        Debug.LogError(gameObject);
    }


    private void OnDisable()
    {
        if (!base.IsOwner)
            return;
        Binds?.Dispose();
        Binds?.Disable();
    }
}