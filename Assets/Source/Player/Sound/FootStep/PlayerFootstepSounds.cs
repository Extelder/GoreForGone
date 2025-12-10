using System;
using System.Collections;
using System.Collections.Generic;
using EvolveGames;
using FishNet.Object;
using UniRx;
using UnityEngine;

public class PlayerFootstepSounds : NetworkBehaviour
{
    [SerializeField] private HeadBob _headBob;
    [SerializeField] private PlayerSoundPlayAndMix _playerSoundPlayAndMix;
    [SerializeField] private float _checkRate;
    
    private CompositeDisposable _checkDisposable = new CompositeDisposable();
    private CompositeDisposable _soundPlayDisposable = new CompositeDisposable();
    
    public override void OnStartClient()
    {
        base.OnStartClient();
        _headBob.Moving.Subscribe(_ =>
        {
            if (_headBob.Moving.Value)
            {
                OnMoving();
                return;
            }
            _soundPlayDisposable.Clear();
        }).AddTo(_checkDisposable);
    }

    private void OnMoving()
    {
        Observable.Interval(TimeSpan.FromSeconds(_checkRate)).Subscribe(_ =>
        {
            _playerSoundPlayAndMix.SoundPlayServer();
        }).AddTo(_soundPlayDisposable);
    }

    private void OnDisable()
    {
        _checkDisposable.Clear();
        _soundPlayDisposable.Clear();
    }
}
