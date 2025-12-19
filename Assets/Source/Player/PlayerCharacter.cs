using System;
using System.Collections;
using System.Collections.Generic;
using EvolveGames;
using FishNet.Object;
using MilkShake;
using UnityEngine;

public class PlayerCharacter : NetworkBehaviour
{
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public PlayerController PlayerController { get; private set; }
    [field: SerializeField] public PlayerHitBox PlayerHitBox { get; private set; }
    [field: SerializeField] public List<PlayerCharacter> Characters { get; private set; }
    [field: SerializeField] public Transform DropPoint { get; private set; }
    [field: SerializeField] public Transform LookAtPoint { get; private set; }
    [field: SerializeField] public PlayerBinds Binds;
    [field: SerializeField] public Transform PlayerTransform;
    [field: SerializeField] public Transform Camera;
    [field: SerializeField] public Shaker Shaker { get; private set; }
    [field: SerializeField] public GameObject[] _thirdPerson;

    public static PlayerCharacter Instance { get; private set; }

    public event Action ClientStarted;

    public static event Action<float> TimeValueChanged;

    public float CurrentTime;

    [ServerRpc(RequireOwnership = false)]
    public void ServerSpawnObject(GameObject spawnedObject, Vector3 position, Quaternion rotation)
    {
        GameObject instance = Instantiate(spawnedObject, position, rotation);

        ServerManager.Spawn(instance);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ServerDeSpawnObject(GameObject despawnedObject)
    {
        ServerManager.Despawn(despawnedObject);
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

    public void ChangeTimeValue(float value, PlayerCharacter investigator)
    {
        TimeValueChangedServer(value, investigator);
    }

    [ServerRpc]
    public void TimeValueChangedServer(float value, PlayerCharacter investigator)
    {
        TimeValueChangedObserver(value, investigator);
    }

    [ObserversRpc]
    public void TimeValueChangedObserver(float value, PlayerCharacter investigator)
    {
        PlayerCharacter.Instance.OnTimeValueChanged(value, investigator);
        TimeValueChanged?.Invoke(value);
    }

    public void OnTimeValueChanged(float value, PlayerCharacter investigator)
    {
        CurrentTime = value;
        if (investigator != this)
        {
            if (value == 0)
            {
                PlayerController.canMove = false;
                Binds.Disable();
            }
            else
            {
                PlayerController.canMove = true;
                Binds.Enable();
            }

            Debug.Log("Time Stop");
        }
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

    [ServerRpc(RequireOwnership = false)]
    public void SetObjectEnableServerWithValidation(GameObject needObject, bool enabled)
    {
        SetObjectEnableObserverValidation(needObject, enabled);
    }

    [ObserversRpc]
    public void SetObjectEnableObserverValidation(GameObject gameObject, bool enabled)
    {
        if (gameObject != null)
            gameObject.SetActive(enabled);
    }

    public void Teleport(Vector3 point)
    {
        CharacterController.enabled = false;
        PlayerController.moveDirection.y = 0;
        PlayerTransform.position = point;
        CharacterController.enabled = true;
    }


    private void OnDisable()
    {
        if (!base.IsOwner)
            return;
        Binds?.Dispose();
        Binds?.Disable();
    }
}