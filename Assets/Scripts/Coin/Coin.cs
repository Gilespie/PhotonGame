using Fusion;
using UnityEngine;

public class Coin : NetworkBehaviour
{
    [SerializeField] int _scorePoints = 50;
    bool _collected;

    private void OnTriggerEnter(Collider other)
    {
        if (_collected) return;
        if (!Object.HasStateAuthority) return;

        if (other.TryGetComponent(out Wallet wallet))
        {
            _collected = true;
            wallet.RPC_AddScore(_scorePoints);
            Runner.Despawn(Object);
        }
    }
}