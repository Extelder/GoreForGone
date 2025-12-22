using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class GlowStickItem : NetworkBehaviour
{
    [SerializeField] private Outline[] _outlines;
    [SerializeField] private GameObject _rebindCanvas;
    [SerializeField] private SetActiveObject _setActiveObject;

    [SerializeField] private GameObject _light;

    private void OnMouseDown()
    {
        Enable();
    }

    private void OnMouseEnter()
    {
        SetOutline(true);
        _rebindCanvas.SetActive(true);
    }

    public void SetOutline(bool value)
    {
        for (int i = 0; i < _outlines.Length; i++)
        {
            _outlines[i].enabled = value;
        }
    }

    private void OnMouseExit()
    {
        SetOutline(false);
        _rebindCanvas.SetActive(false);
    }

    public void Enable()
    {
        _setActiveObject.SetActiveServer(_light, !_light.active);
    }
}