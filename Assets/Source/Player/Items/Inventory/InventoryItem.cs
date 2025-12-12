using System;
using System.Collections;
using System.Collections.Generic;
using EvolveGames;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private HandsSmooth _handsSmooth;

    private void OnEnable()
    {
        _handsSmooth.enabled = false;
        GameCursor.Instance.Show();
    }

    private void OnDisable()
    {
        _handsSmooth.enabled = true;
        GameCursor.Instance.ToPrevState();
    }
}