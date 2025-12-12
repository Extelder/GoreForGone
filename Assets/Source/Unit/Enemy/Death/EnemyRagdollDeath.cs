using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using NTC.Global.System;
using UnityEngine;

public class EnemyRagdollDeath : NetworkBehaviour
{
    [SerializeField] private SetActiveObject _setActiveObject;
    [SerializeField] private EnemyAnimator _enemyAnimator;
    [SerializeField] private Transform _ragdollParent;
    [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshRenderer;
    [SerializeField] private Transform _headBone;
    [SerializeField] private GameObject _head;
    [SerializeField] private NetworkObject _enemyParent;
    [SerializeField] private RagdollOperations _ragdollOperations;
    [SerializeField] private float _explosionForce = 100;

    public void Death(Transform explosionPoint)
    {
        StopAllCoroutines();
        _ragdollParent.parent = null;
        _ragdollParent.SetParent(null);

        _enemyAnimator.DisableAllBools();
        _ragdollOperations.EnableRagdoll();

        int rand = Random.Range(0, 20);
        var headChance = rand >= 17;
        if (headChance)
        {
            _headBone.transform.localScale = Vector3.zero;
            _setActiveObject.SetActiveServer(_head, true);
        }

        _ragdollOperations.AddExplosionForce(_explosionForce, explosionPoint.position, 20, 1,
            ForceMode.Impulse);


        for (int i = 0; i < _skinnedMeshRenderer.Length; i++)
        {
            _skinnedMeshRenderer[i].updateWhenOffscreen = true;
        }

        StartCoroutine(DestroyingRagdoll());
    }    
    
    public void Death(Vector3 explosionPoint)
    {
        StopAllCoroutines();
        _ragdollParent.parent = null;
        _ragdollParent.SetParent(null);

        _enemyAnimator.DisableAllBools();
        _ragdollOperations.EnableRagdoll();

        int rand = Random.Range(0, 20);
        var headChance = rand >= 17;
        if (headChance)
        {
            _headBone.transform.localScale = Vector3.zero;
            _setActiveObject.SetActiveServer(_head, true);
        }

        _ragdollOperations.AddExplosionForce(_explosionForce, explosionPoint, 20, 1,
            ForceMode.Impulse);

        for (int i = 0; i < _skinnedMeshRenderer.Length; i++)
        {
            _skinnedMeshRenderer[i].updateWhenOffscreen = true;
        }
        
        StartCoroutine(DestroyingRagdoll());
    }

    private IEnumerator DestroyingRagdoll()
    {
        yield return new WaitForSeconds(10);
        Despawn(_enemyParent);
    }
}
