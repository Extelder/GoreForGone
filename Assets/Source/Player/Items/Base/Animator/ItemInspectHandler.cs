using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemInspectHandler : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _character;
    [SerializeField] private StateMachine _itemMachine;
    [SerializeField] private State[] _ignoreStates;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _inspectAnimationBool = "IsInspecting";


    private void OnStateChanged(State state)
    {
        for (int i = 0; i < _ignoreStates.Length; i++)
        {
            if (_ignoreStates[i] == state)
                return;
        }

        _animator.SetBool(_inspectAnimationBool, false);
    }

    private bool _initialized;

    private void OnEnable()
    {
        _itemMachine.StatePrepareToChange += OnStateChanged;

        if (_initialized)
        {
            _character.Binds.Character.Inspect.performed += OnInpectPerformed;
        }

        _character.ClientStarted += OnClientStarted;
    }

    private void OnInpectPerformed(InputAction.CallbackContext obj)
    {
        _animator.SetBool(_inspectAnimationBool, true);
    }

    private void OnClientStarted()
    {
        _initialized = true;
        _character.Binds.Character.Inspect.performed += OnInpectPerformed;
    }

    private void OnDisable()
    {
        _itemMachine.StatePrepareToChange -= OnStateChanged;

        _character.Binds.Character.Inspect.performed -= OnInpectPerformed;
        _character.ClientStarted -= OnClientStarted;
    }
}