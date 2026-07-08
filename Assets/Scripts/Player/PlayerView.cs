using Fusion;
using UnityEngine;

public class PlayerView : NetworkBehaviour
{
    [SerializeField] private GameObject _playerVisual;
    [SerializeField] CharacterAnimationController _animationController;
    [SerializeField] CharacterColliderResizer _characterColliderResizer;
    [SerializeField] MovementComponent _movement;
    [SerializeField] HealthSystem _healthSystem;
    [SerializeField] Ragdoll _ragdoll;
    [SerializeField] ParticleSystem _bloodVFX;

    private int _lastJumpCounter;
    private bool _lastCrouchState;

    public override void Spawned()
    {
        _ragdoll.DisableRagdoll();

        _healthSystem.OnDeadChanged += OnDeadStateChanged;
        _healthSystem.OnRespawn += OnRespawned;
        _healthSystem.OnHit += OnHited;

        _lastJumpCounter = _movement.JumpCounter;
        _lastCrouchState = _movement.IsCrouching;
    }

    public override void Render()
    {
        _animationController.SetFloat(AnimParams.Speed, _movement.MovementMagnitude);
        _animationController.SetBool(AnimParams.Air, !_movement.IsGrounded);

        if (_movement.JumpCounter != _lastJumpCounter)
        {
            _lastJumpCounter = _movement.JumpCounter;
            _animationController.SetTrigger(AnimParams.Jump);
        }

        if (_movement.IsCrouching != _lastCrouchState)
        {
            _lastCrouchState = _movement.IsCrouching;
            ApplyCrouch(_movement.IsCrouching);
        }
    }

    private void ApplyCrouch(bool isCrouching)
    {
        if (isCrouching)
        {
            _characterColliderResizer.SetSize(1, new Vector3(0, -0.5f, 0));
            _animationController.SetBool(AnimParams.Crouch, true);
        }
        else
        {
            _characterColliderResizer.SetSize(2, new Vector3(0, 0, 0));
            _animationController.SetBool(AnimParams.Crouch, false);
        }
    }

    void OnDeadStateChanged(bool isDead)
    {
        if (isDead)
            _ragdoll.ActivateRagdoll();
        else
            _ragdoll.DisableRagdoll();
    }

    void OnRespawned()
    {
        _ragdoll.DisableRagdoll();
    }

    void OnHited()
    {
        _bloodVFX.Play();
    }
}