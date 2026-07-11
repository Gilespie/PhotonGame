using Fusion;
using UnityEngine;

public class FinishTrigger : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority) return;
        if (!other.TryGetComponent(out Player player)) return;

        GameManager.Instance.PlayerEnteredFinish(player.Object.InputAuthority);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!HasStateAuthority) return;
        if (!other.TryGetComponent(out Player player)) return;

        GameManager.Instance.PlayerExitedFinish(player.Object.InputAuthority);
    }
}