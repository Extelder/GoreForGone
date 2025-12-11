using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitMultiAnimator : MonoBehaviour
{
    [field: SerializeField] public Animator[] Animator { get; private set; }

    public abstract void DisableAllBools();

    public virtual void Idle()
    {
        DisableAllBools();
    }

    public void SetAnimationBoolAndDisableOther(string name)
    {
        DisableAllBools();
        SetAnimationBool(name, true);
    }

    public void SetAnimationBool(string name, bool value)
    {
        for (int i = 0; i < Animator.Length; i++)
        {
            Animator[i].SetBool(name, value);
        }
    }

    public void SetAnimationInt(string name, int value)
    {
        for (int i = 0; i < Animator.Length; i++)
        {
            Animator[i].SetInteger(name, value);
        }
    }

    public void SetAnimationTrigger(string name)
    {
        for (int i = 0; i < Animator.Length; i++)
        {
            Animator[i].SetTrigger(name);
        }
    }

    public void ResetAnimationTrigger(string name)
    {
        for (int i = 0; i < Animator.Length; i++)
        {
            Animator[i].ResetTrigger(name);
        }
    }
}