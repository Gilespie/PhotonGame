using Fusion;
using UnityEngine;

public class CharacterAnimationController : NetworkBehaviour
{
    [Header("Animator")]
    [SerializeField] NetworkMecanimAnimator _animator;

    public void SetBool(int param, bool value)
    {
        _animator.Animator.SetBool(param, value);
    }

    public void SetFloat(int param, float value)
    {
        _animator.Animator.SetFloat(param, value);
    }

    public void SetTrigger(int param)
    {
        _animator.Animator.SetTrigger(param);
    }
}