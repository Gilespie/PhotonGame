using Fusion;
using Fusion.Addons.Physics;
using System;
using UnityEngine;

public class MovementComponent : NetworkBehaviour
{
    [Header("Speed")]
    [SerializeField] float _defaultSpeed = 5f;
    [SerializeField] float _crouchSpeed = 3f;
    [SerializeField] float _sprintSpeed = 8f;
    [SerializeField] float _jumpForce = 5f;
    float _currentSpeed { get; set; }

    [Header("References")]
    [SerializeField] NetworkRigidbody3D _rb;

    public event Action OnInteracting = delegate { };

    [Networked] public int JumpCounter { get; private set; }
    [Networked] public bool IsCrouching { get; private set; }
    [Networked] public bool IsGrounded { get; private set; }
    [Networked] public float MovementMagnitude { get; private set; }


    private void Awake()
    {
        _currentSpeed = _defaultSpeed;
    }

    public void SetGrounded(bool isGrounded)
    {
        IsGrounded = isGrounded;
    }

    public void Movement(Vector3 direction, NetworkRunner runner)
    {
        Vector3 vel = _rb.Rigidbody.linearVelocity;
        Vector3 horizontal = direction.normalized * _currentSpeed;
        vel.x = horizontal.x;
        vel.z = horizontal.z;
        _rb.Rigidbody.linearVelocity = vel;

        MovementMagnitude = Mathf.Clamp01(direction.magnitude) * _currentSpeed;
    }

    public void Jump(bool isGrounded)
    {
        if (!isGrounded) return;

        Vector3 velocity = _rb.Rigidbody.linearVelocity;
        velocity.y = _jumpForce;
        _rb.Rigidbody.linearVelocity = velocity;

        if (HasStateAuthority) JumpCounter++;
    }

    public void SetCrouch(bool isCrouching)
    {
        _currentSpeed = isCrouching ? _crouchSpeed : _defaultSpeed;
        IsCrouching = isCrouching;
    }

    public void SetSprint(bool isSprinting)
    {
        _currentSpeed = isSprinting ? _sprintSpeed : _defaultSpeed;
    }
}