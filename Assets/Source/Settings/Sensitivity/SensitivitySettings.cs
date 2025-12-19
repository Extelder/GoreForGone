using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using EvolveGames;
using FishNet.Object;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySettings : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Slider _sensetivitySlider;
    
    private CinemachinePOV _cinemachinePOV;
    private PlayerConfig _config;

    private void Start()
    {
        _config = PlayerConfig.Instance;
        _playerController.SensetivityMultiplier = _config.ConfigData.lookSensitivity;
        _sensetivitySlider.value = _config.ConfigData.lookSensitivity;
    }

    public void SetSensitivity(float value)
    {
        _playerController.SensetivityMultiplier = value;
        _config.ConfigData.lookSensitivity = value;
        _config.Save();
    }
}
