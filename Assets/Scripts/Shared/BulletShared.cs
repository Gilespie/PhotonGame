using Fusion;
using UnityEngine;

public class BulletShared : NetworkBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _lifeTime;
    TickTimer _lifeTimer = TickTimer.None;

    public override void Spawned()
    {
        if (!HasStateAuthority) return;
        
        _lifeTimer = TickTimer.CreateFromSeconds(Runner, _lifeTime);
    }

    public override void FixedUpdateNetwork()
    {
        Move();

        if (_lifeTimer.Expired(Runner))
        {
            _lifeTimer = TickTimer.None;

            Runner.Despawn(Object);
        }
    }

    void Move()
    {
        transform.position += transform.forward * _speed * Runner.DeltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!Object || !Object.HasStateAuthority) return;

        if (other.TryGetComponent(out HealthSystem health) && ShouldDamage(health))
        {
            health.RPC_TakeDamage(_damage);
        }

        Runner.Despawn(Object);
    }

    bool ShouldDamage(HealthSystem targetHealth)
    {
        if (!targetHealth.TryGetComponent(out Player _)) return true;

        bool friendlyFire = Runner.SessionInfo.Properties.TryGetValue("FriendlyFire", out var value) && (bool)value;

        return friendlyFire;
    }
}