using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class TorSpawn : NetworkBehaviour
{
    [SerializeField] private GameObject _models;

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

    private IEnumerator Hit()
    {
        while (true)
        {
            _models.transform.localPosition += Vector3.forward * 9f * Time.deltaTime;
            _models.transform.localScale += Vector3.one * 2f * Time.deltaTime;
            if (_models.transform.localPosition.magnitude >= 100f)
            {
                Despawn();
                yield break;
            }

            yield return null;
        }
    }

    private void Despawn()
    {
        if (IsServer) PlayerCharacter.Instance.Despawn();
    }
}