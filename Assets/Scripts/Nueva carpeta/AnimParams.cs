using UnityEngine;

public static class AnimParams
{
    public static readonly int Move = Animator.StringToHash("isMoving");
    public static readonly int Ground = Animator.StringToHash("isGrounded");
    public static readonly int Air = Animator.StringToHash("isOnAir");
    public static readonly int Crouch = Animator.StringToHash("isCrouch");
    public static readonly int Speed = Animator.StringToHash("moveSpeed");
    public static readonly int Jump = Animator.StringToHash("onJump");
    public static readonly int Press = Animator.StringToHash("onPressed");
    public static readonly int Attack = Animator.StringToHash("isAttacking");

}