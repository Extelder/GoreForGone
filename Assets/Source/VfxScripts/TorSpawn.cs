using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class TorSpawn : NetworkBehaviour
{
    [SerializeField] private GameObject _models;
    private Vector3 _startPos;

    public override void OnStartClient()
    {
        Play();
    }


    [ServerRpc(RequireOwnership = false)]
    private void Play()
    {
        if (IsServer) PlayObserver();
    }

    [ObserversRpc]
    private void PlayObserver()
    {
        StartCoroutine(Hit());
    }
    private void OnEnable()
    {
        _startPos = transform.position;
    }
    private IEnumerator Hit()
    {
        while (true)
        {
            _models.transform.position += _models.transform.forward * 4.2f * Time.deltaTime;
            _models.transform.localScale += Vector3.one * 100f * Time.deltaTime;
            if (Vector3.Distance(_models.transform.position, _startPos) >= 12f)
            {
                Despawn();
                yield break;
            }

            yield return null;
        }
    }

    private void Despawn()
    {
        if (IsServer) PlayerCharacter.Instance.ServerDeSpawnObject(gameObject);
    }
}