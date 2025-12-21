using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractItemWithHighlight : InteractItem
{
    [SerializeField] private MeshRenderer _meshRenderer;

    private Material[] _defaultMaterials;

    private bool _detected;

    public override void Interact()
    {
        Item.Interact();
    }

    public override void Detected()
    {
        if (_detected)
            return;

        _detected = true;

        _defaultMaterials = _meshRenderer.materials;

        Material[] mats = new Material[_defaultMaterials.Length + 1];
        for (int i = 0; i < _defaultMaterials.Length; i++)
        {
            mats[i] = _defaultMaterials[i];
        }

        mats[mats.Length - 1] = PlayerCharacter.Instance.InteractMaterial;

        _meshRenderer.materials = mats;
    }

    public override void Lost()
    {
        if (!_detected)
            return;
        _detected = false;
        _meshRenderer.materials = _defaultMaterials;
    }
}