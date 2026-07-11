using UnityEngine;

public class BridgeTrigger : MonoBehaviour
{
    [SerializeField] Bridge _bridge;
    [SerializeField] bool _activateOnEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _bridge.RPC_SetActivated(_activateOnEnter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _bridge.RPC_SetActivated(!_activateOnEnter);
        }
    }
}
