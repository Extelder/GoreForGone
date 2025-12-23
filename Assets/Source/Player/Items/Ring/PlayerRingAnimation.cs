using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRingAnimation : PlayerRing
{
    [SerializeField] private string _idleAnimName;

    [SerializeField] private string _beginAnimName;
    [SerializeField] private string _stayAnimName;
    [SerializeField] private string _performedAnimName;

    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerAnimator _tpsAnimator;

    protected override void OnRingAbilityBindCanceled(InputAction.CallbackContext obj)
    {
        _animator.Play(_performedAnimName);
        _tpsAnimator.PlayAnimationServer(_performedAnimName, 2);
    }

    protected override void OnRingAbilityBindPerformed(InputAction.CallbackContext obj)
    {
    }

    protected override void OnRingAbilityBindStarted(InputAction.CallbackContext obj)
    {
        _animator.Play(_beginAnimName);
        _tpsAnimator.PlayAnimationServer(_beginAnimName, 2);

        _animator.CrossFade(_stayAnimName, 0.5f);
        _tpsAnimator.CrossfadeAnimationServer(_stayAnimName, 0.5f, 2);
    }

    protected override void CancelAction()
    {
    }
}