using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSItem : MonoBehaviour
{
    [SerializeField] private GameObject _tpsItem;

    private void OnEnable()
    {
        _tpsItem.SetActive(true);
    }

    private void OnDisable()
    {
        _tpsItem.SetActive(false);
    }
}