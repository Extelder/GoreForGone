using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRingAnimation : PlayerRing
{
    [SerializeField] private string _idleAnimName;

    [SerializeField] private string _beginAnimName;
    [SerializeField] private string _stayAnimName;
    [SerializeField] private string _performedAnimName;

    [SerializeField] private Animator _animator;

    protected override void OnRingAbilityBindCanceled(InputAction.CallbackContext obj)
    {
        _animator.Play(_performedAnimName);
    }

    protected override void OnRingAbilityBindPerformed(InputAction.CallbackContext obj)
    {
    }

    protected override void OnRingAbilityBindStarted(InputAction.CallbackContext obj)
    {
        _animator.Play(_beginAnimName);

        _animator.CrossFade(_stayAnimName, 0.5f);
    }

    protected override void CancelAction()
    {
    }
}