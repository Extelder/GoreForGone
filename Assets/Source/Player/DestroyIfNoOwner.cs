using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfNoOwner : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _character;

    private void OnEnable()
    {
        _character.ClientStarted += OnClienStarted;
    }

    private void OnClienStarted()
    {
        if (!_character.IsOwner)
            Destroy(gameObject);
    }

    private void OnDisable()
    {
        _character.ClientStarted -= OnClienStarted;
    }
}