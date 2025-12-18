using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class TorSpawn : NetworkBehaviour
{
    [SerializeField] private float _force = 6f;
    [SerializeField] private GameObject _models;
    private Vector3 _startPos;
    static readonly int AlphaID = Shader.PropertyToID("_Alpha");
    Renderer rend;
    MaterialPropertyBlock mpb;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
    }

    public void SetAlpha(float alpha)
    {
        rend.GetPropertyBlock(mpb);
        mpb.SetFloat(AlphaID, alpha);
        rend.SetPropertyBlock(mpb);
    }

    public override void OnStartClient()
    {
        Play();
    }


    [ServerRpc(RequireOwnership = false)]
    private void Play()
    {
        if (IsServer) PlayObserver();
        PlayerCharacter.Instance.PlayerController.AddImpulse(-PlayerCharacter.Instance.Camera.forward, _force);
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
            _models.transform.position += _models.transform.forward * 10f * Time.deltaTime;
            _models.transform.localScale += Vector3.one * 280f * Time.deltaTime;
            float dist = Vector3.Distance(_models.transform.position, _startPos);
            float t = Mathf.Clamp01(dist / 9f);
            SetAlpha(Mathf.Lerp(1f, 0f, t));
            if (Vector3.Distance(_models.transform.position, _startPos) >= 9f)
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