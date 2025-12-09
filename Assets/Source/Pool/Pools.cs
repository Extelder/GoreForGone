using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pools : MonoBehaviour
{
    public static Pools Instance { get; private set; }
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            return;
        }
        Debug.LogError("Theres one more Pools");
    }
    
    [field:SerializeField] public Pool BloodExplodePool { get; private set; }
    [field:SerializeField] public Pool BloodPool { get; private set; }
}