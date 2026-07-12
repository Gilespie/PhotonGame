using Fusion;
using Fusion.Addons.Physics;
using System;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    public Action OnEnemyDead = delegate { };

    [SerializeField] float _speedMin = 5f;
    [SerializeField] float _speedMax = 5f;
    [SerializeField] float _rotationSpeed = 10f;

    [SerializeField] NetworkRigidbody3D _rb;
    [SerializeField] NetworkMecanimAnimator _mecanimAnimator;
    [SerializeField] HealthSystem _health;
    [SerializeField] ParticleSystem _bloodVFX;
    [SerializeField] Ragdoll _ragdoll;

    [SerializeField] float _despawnInterval = 15f;
    [Networked] TickTimer _tickTimer { get; set; }
    float _currentSpeed;

    [Header("Attack")]
    [SerializeField] float _damage = 25f;
    bool _isAttacking;


    [SerializeField]float stoppingDistance = 2f;
    Player _player;
    float sqrtDistance;
    bool _despawnQueued;

    public override void Spawned()
    {
        _currentSpeed = UnityEngine.Random.Range(_speedMin, _speedMax);
        _health.OnDeadChanged += OnDeadStateChanged;
        _health.OnHit += OnHited;
    }

    public override void FixedUpdateNetwork()
    {
        if (_health.IsDead)
        {
            if (!_despawnQueued && _tickTimer.Expired(Runner))
            {
                _despawnQueued = true;
                Runner.Despawn(Object);
            }
            return;
        }
        

        sqrtDistance = (_player.transform.position - _rb.Rigidbody.position).sqrMagnitude;

        Vector3 directionToPlayer = _player.transform.position - _rb.Rigidbody.position;
        RotateTowards(directionToPlayer);

        if (sqrtDistance > stoppingDistance * stoppingDistance)
        {
            MoveTo();
        }
        else
        {
            Attack();
        }
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        _health.OnDeadChanged -= OnDeadStateChanged;
        _health.OnHit -= OnHited;
    }

    void OnDeadStateChanged(bool isDead)
    {
        if (!isDead) return;

        _ragdoll.ActivateRagdoll(); 

        if (!HasStateAuthority) return;

        OnEnemyDead();
        _tickTimer = TickTimer.CreateFromSeconds(Runner, _despawnInterval);
    }

    void OnHited()
    {
        _bloodVFX.Play();
    }

    public void SetTarget(Player player)
    {
        _player = player;
    }

    private void Attack()
    {
        if (_isAttacking) return;

        SetBoolAttacking(true);
    }

    public void ApplyDamage()
    {
        if (!HasStateAuthority) return;
        if (_player == null) return;

        float currentSqrDistance = (_player.transform.position - _rb.Rigidbody.position).sqrMagnitude;

        if (currentSqrDistance > stoppingDistance * stoppingDistance) return;

        if (_player.TryGetComponent<HealthSystem>(out var health))
            health.RPC_TakeDamage(_damage);
    }

    void MoveTo()
    {
        if(_player == null) return;

        Vector3 direction = _player.transform.position - _rb.Rigidbody.position;

        _rb.Rigidbody.MovePosition(_rb.Rigidbody.position + direction.normalized * _currentSpeed * Runner.DeltaTime);

        _mecanimAnimator.Animator.SetFloat(AnimParams.Speed, _currentSpeed);
    }

    public void SetBoolAttacking(bool attacking)
    {
        _isAttacking = attacking;
        _mecanimAnimator.Animator.SetBool(AnimParams.Attack, attacking);
    }

    void RotateTowards(Vector3 direction)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion newRotation = Quaternion.Slerp(_rb.Rigidbody.rotation, targetRotation, _rotationSpeed * Runner.DeltaTime);

        _rb.Rigidbody.MoveRotation(newRotation);
    }
}