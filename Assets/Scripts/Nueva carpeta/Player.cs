using System;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [Header("Speed")]
    [SerializeField] float _defaultSpeed = 3f;
    [SerializeField] float _sprintSpeed = 6f;
    [SerializeField] float _jumpForce = 5f;

    [Header("Lifes")]
    [SerializeField] int _maxLife;
    [Networked] float Health { get; set; }
    [Networked] public float _currentHealth { get; private set; }

    [Header("Crouch")]
    [SerializeField] float _crouchSpeed = 3f;
    [SerializeField] float _crouchColHeight = 1f;
    [SerializeField] float _crouchColCenter = 1f;

    [Header("Shoot")]
    [SerializeField] float _fireRate = 0.5f;
    float _timer;

    [SerializeField] Ragdoll _ragdoll;
    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] Transform _bulletSpawnerTransform;
    [SerializeField] CharacterColliderResizer _colliderResizer;
    [SerializeField] CharacterRotator _characterRotator;
    [SerializeField] CharacterInputController _inputController;
    [SerializeField] GroundRaycast _groundRaycast;
    [SerializeField] InteractRaycast _interactRaycast;
    //[SerializeField] CharacterMeshSelector _meshSelector;
    [SerializeField] CharacterAnimationController _animationController;

    bool _isGround;
    //bool _isInteract;
    bool _isJumping;
    bool _isShooting;
    bool _canShootNow;
    bool _isCrouching;
    bool _isRagdoll;
    bool _isOnAir;
    Camera _camera;
    NetworkRigidbody3D _rb;
    Vector3 _direction;
    //int _randomMeshIndex = 0;

    public override void Spawned()
    {
        _rb = GetComponent<NetworkRigidbody3D>();

        _currentHealth = _maxLife;
        _timer = _fireRate;
        
        _ragdoll.DisableRagdoll();

        /*_randomMeshIndex = UnityEngine.Random.Range(0, _meshSelector.MecanimAnims.Length);

         _meshSelector.SelectMesh(_randomMeshIndex);
         _animationController.SetAnimator(_meshSelector.MecanimAnims[_randomMeshIndex]);*/

        if (HasStateAuthority)
        {
            _camera = Camera.main;
            _camera.GetComponent<FollowTarget>().SetTarget(this);
        }

        GameManager.Instance.AddToList(this);
    }

    void Update()
    {
        _inputController.ArtificialUpdate();

        _isGround = _groundRaycast.IsRaycasting(Vector3.down);
        //_isInteract = _interactRaycast.IsRaycasting(_characterRotator.transform.forward);

        if(_inputController.IsJumpPressed && _isGround)
        {
            _isJumping = true;
        }

        if (_inputController.IsCrouchPressed)
        {
            _isCrouching = true;
        }
        else
        {
            _isCrouching = false;
        }
        
        _isShooting = _inputController.IsShootPressed;
        _direction = _inputController.DirectionPressed;
        _isRagdoll = _inputController.IsRagdollPressed;

        _timer -= Runner.DeltaTime;
        _timer = Mathf.Clamp(_timer, 0, _fireRate);

        _canShootNow = _timer <= 0f;

        _isOnAir = !_isGround;

        _animationController.SetBool(AnimParams.Air, _isOnAir);
    }
        
    public override void FixedUpdateNetwork()
    {
        Movement();

        if(_isRagdoll)
        {
            RPC_ActivateRagdoll(true);
        }
        else
        {
            RPC_ActivateRagdoll(false);
        }

        if (_isJumping && !_isCrouching)
        {
            Jump();
        }

        if (_canShootNow && _isShooting)
        {
            SpawnShot();
            _timer = _fireRate;
        }

        if (_isCrouching && !_isJumping)
        {
            Crouch();
        }
        else
        {
            Uncrouch();
        }

        _characterRotator.RotateDefault(_inputController.DirectionPressed);
    }

    void Movement()
    {
        Vector3 velocity = _direction.normalized * _defaultSpeed;

        _rb.Rigidbody.MovePosition(_rb.Rigidbody.position + velocity * Runner.DeltaTime);

        _animationController.SetFloat(AnimParams.Speed, velocity.magnitude);
    }

    void Jump()
    {
        if(!_isJumping) return;

        _animationController.SetTrigger(AnimParams.Jump);
        _rb.Rigidbody.linearVelocity = Vector3.up * _jumpForce;
        _isJumping = false;
    }

    void SpawnShot()
    {
        Runner.Spawn(_bulletPrefab, _bulletSpawnerTransform.position, _bulletSpawnerTransform.rotation, Object.InputAuthority);
    }

    void Crouch()
    {
        _colliderResizer.SetSize(1, new Vector3(0, -0.5f, 0));
        _animationController.SetBool(AnimParams.Crouch, true);
    }

    void Uncrouch()
    {
        _colliderResizer.SetSize(2, new Vector3(0, 0, 0));
        _animationController.SetBool(AnimParams.Crouch, false);
    }

    //Hacer una funcion local para recibir daño y que llame a la funcion de morir cuando la vida sea <= 0
    void TakeDamage(float damage)
    {
        if (!HasStateAuthority) return;

        if (damage <= 0) return;

        _currentHealth = MathF.Max(_currentHealth - damage, 0);

        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    //Hacer una funcion networkeada para recibir daño que llame a la funcion local de recibir daño.
    [Rpc]
    public void RPC_TakeDamage(float damage)
    {
        TakeDamage(damage);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ActivateRagdoll(bool activate)
    {
        if (activate)
        {
            _ragdoll.ActivateRagdoll();
        }
        else
        {
            _ragdoll.DisableRagdoll();
        }
    }

    void Death()
    {
        //Llamo a la funcion de derrota del game manager y paso mi local player
        GameManager.Instance.RPC_Defeat(Runner.LocalPlayer);

        RPC_ActivateRagdoll(true);

        //Runner.Despawn(Object);
    }
}