using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class RingPostProccessVolume : PlayerRing
{
    [SerializeField] private PostProcessVolume _postProcessVolume;
    [SerializeField] private PostProcessProfile _postProcessProfile;
    [SerializeField] private float _weightFadeSpeed;

    private CompositeDisposable _disposable = new CompositeDisposable();

    protected override void OnRingAbilityBindStarted(InputAction.CallbackContext obj)
    {
        _postProcessVolume.weight = 0f;
        _postProcessVolume.profile = _postProcessProfile;

        _disposable?.Clear();

        Observable.EveryUpdate()
            .TakeWhile(_ => _postProcessVolume.weight < 1f)
            .Subscribe(_ =>
                {
                    _postProcessVolume.weight = Mathf.MoveTowards(
                        _postProcessVolume.weight,
                        1f,
                        _weightFadeSpeed * Time.deltaTime
                    );
                },
                () => _postProcessVolume.weight = 1f)
            .AddTo(_disposable);
    }

    protected override void OnRingAbilityBindCanceled(InputAction.CallbackContext obj)
    {
        _disposable?.Clear();
        Observable.EveryUpdate()
            .TakeWhile(_ => _postProcessVolume.weight > 0f)
            .Subscribe(_ =>
                {
                    _postProcessVolume.weight = Mathf.MoveTowards(
                        _postProcessVolume.weight,
                        0f,
                        _weightFadeSpeed * Time.deltaTime
                    );
                },
                () => _postProcessVolume.weight = 0f)
            .AddTo(_disposable);
    }

    protected override void CancelAction()
    {
        _postProcessVolume.weight = 0f;
        _disposable?.Clear();
    }
}