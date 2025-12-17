using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class TimeStopRing : PlayerRing
{
    [SerializeField] private PostProcessVolume _postProcessVolume;

    [SerializeField] private float _timeStopTime;

    protected override void OnRingAbilityBindCanceled(InputAction.CallbackContext obj)
    {
        PlayerCharacter.Instance.ChangeTimeValue(0, PlayerCharacter.Instance);
        _postProcessVolume.weight = 1;
        StartCoroutine(WaitingForRecover());
    }

    private IEnumerator WaitingForRecover()
    {
        yield return new WaitForSeconds(_timeStopTime);
        PlayerCharacter.Instance.ChangeTimeValue(1, PlayerCharacter.Instance);
        _postProcessVolume.weight = 0;
    }

    protected override void CancelAction()
    {
        StopAllCoroutines();
        PlayerCharacter.Instance.ChangeTimeValue(1, PlayerCharacter.Instance);
        _postProcessVolume.weight = 0;
    }
}