using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlowStickLight : NetworkBehaviour
{
    [SerializeField] private SetActiveObject _setActiveObject;
    [SerializeField] private GameObject _light;
    
    public override void OnStartClient()
    {
        if (!base.IsOwner)
            return;
        base.OnStartClient();
        PlayerCharacter.Instance.Binds.Character.GlowStick.performed += OnGlowStickPerformed;
    }

    private void OnGlowStickPerformed(InputAction.CallbackContext obj)
    {
        _setActiveObject.SetActiveServer(_light, !_light.activeSelf);
    }

    private void OnDisable()
    {
        if (!base.IsOwner)
            return;
        PlayerCharacter.Instance.Binds.Character.GlowStick.performed -= OnGlowStickPerformed;
    }
}