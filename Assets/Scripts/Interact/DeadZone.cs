using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] float _damage = 1000f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HealthSystem health))
        {
            if (!health.Object.HasStateAuthority) return;

            health.RPC_TakeDamage(_damage);
        }
    }
}