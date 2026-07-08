using Fusion;
using System;
using UnityEngine;

public class HealthSystem : NetworkBehaviour
{
    [SerializeField] int _maxLife;
    [Networked, OnChangedRender(nameof(OnHealthChanged))]
    public float CurrentHealth { get; private set; }

    [Networked, OnChangedRender(nameof(OnDeadStateChanged))]
    public NetworkBool IsDead { get; private set; }

    public event Action OnDead = delegate { };
    public event Action<float> OnHealthUpdate = delegate { };
    public event Action<bool> OnDeadChanged = delegate { };
    public event Action OnRespawn = delegate { };
    public event Action OnHit = delegate { };

    public event Action OnLeft = delegate { };

    public override void Spawned()
    {
        if (HasStateAuthority)
            CurrentHealth = _maxLife;
        else
            OnHealthChanged();
    }

    void OnHealthChanged()
    {
        OnHealthUpdate(CurrentHealth / _maxLife);
    }

    void OnDeadStateChanged()
    {
        OnDeadChanged(IsDead);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_TakeDamage(float damage)
    {
        TakeDamage(damage);
    }

    void TakeDamage(float damage)
    {
        if (!HasStateAuthority) return;

        if (damage <= 0 || IsDead) return;

        CurrentHealth = MathF.Max(CurrentHealth - damage, 0);
        OnHit();

        if (CurrentHealth <= 0)
        {
            IsDead = true;
            OnDead();
        }
    }

    public void Resurrect()
    {
        if (!HasStateAuthority) return;

        CurrentHealth = _maxLife;
        IsDead = false;
        OnRespawn();
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnLeft();
    }
}