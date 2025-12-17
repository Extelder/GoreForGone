using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class TimeStopRing : PlayerRing
{
    [SerializeField] private PostProcessVolume _postProcessVolume;

    [SerializeField] private float _timeStopTime;

    private CompositeDisposable _disposable = new CompositeDisposable();

    protected override void OnRingAbilityBindCanceled(InputAction.CallbackContext obj)
    {
        if (PlayerCharacter.Instance.CurrentTime == 0)
        {
            PlayerCharacter.Instance.ChangeTimeValue(1, PlayerCharacter.Instance);
            _disposable?.Clear();
        }

        PlayerCharacter.Instance.ChangeTimeValue(0, PlayerCharacter.Instance);
        _postProcessVolume.weight = 1;
        Observable.Timer(TimeSpan.FromSeconds(6)).Subscribe(_ =>
        {
            PlayerCharacter.Instance.ChangeTimeValue(1, PlayerCharacter.Instance);
            _postProcessVolume.weight = 0;
        }).AddTo(_disposable);
    }


    protected override void CancelAction()
    {
    }
}