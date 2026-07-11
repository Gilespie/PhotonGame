using Fusion;
using Fusion.Addons.Physics;
using System.Collections;
using UnityEngine;

public class RespawnSystem : NetworkBehaviour
{
    [SerializeField] HealthSystem _health;
    [SerializeField] byte _maxDeaths = 3;
    [SerializeField] float _delayToRespawn = 3f;
    [SerializeField] NetworkRigidbody3D _rb;
    byte _deathCount;
    Vector3 _initialSpawnPosition;

    public override void Spawned()
    {
        _initialSpawnPosition = transform.position;

        if (!HasStateAuthority) return;

        _health.OnDead += OnDied;
    }

    void OnDied()
    {
        if (!HasStateAuthority) return;

        _deathCount++;

        if (_deathCount >= _maxDeaths)
        {
            DisconnectPlayer();
            return;
        }

        StartCoroutine(RespawnCooldown());
    }

    IEnumerator RespawnCooldown()
    {
        yield return new WaitForSeconds(_delayToRespawn);

        Vector3 respawnPos = GetRespawnPosition();
        TeleportTo(respawnPos);

        _health.Resurrect();
    }

    Vector3 GetRespawnPosition()
    {
        return _initialSpawnPosition;
    }

    void TeleportTo(Vector3 position)
    {
        if (_rb != null)
        {
            if (!_rb.Rigidbody.isKinematic)
                _rb.Rigidbody.linearVelocity = Vector3.zero;

            _rb.Teleport(position, transform.rotation);
        }
        else
        {
            transform.position = position;
        }
    }

    void DisconnectPlayer()
    {
        GameManager.Instance.RPC_Defeat(Object.InputAuthority);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        _health.OnDead -= OnDied;
    }
}