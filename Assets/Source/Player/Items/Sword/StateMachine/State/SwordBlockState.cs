using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordBlockState : SwordState
{
    public override void Enter()
    {
        Animator.Block();
        CanChanged = false;
        PlayerCharacter.Instance.Binds.Character.Block.canceled += OnBlockCancelled;
    }

    public override void Exit()
    {
        PlayerCharacter.Instance.Binds.Character.Block.canceled -= OnBlockCancelled;
    }

    private void OnDisable()
    {
        PlayerCharacter.Instance.Binds.Character.Block.canceled -= OnBlockCancelled;
    }

    private void OnBlockCancelled(InputAction.CallbackContext obj)
    {
        CanChanged = true;
    }
}