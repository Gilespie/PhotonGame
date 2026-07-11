using Fusion;
using UnityEngine;

public class Bridge : NetworkBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] float _openAngle = -37f;
    [SerializeField] float _closeAngle = 0f;

    [Networked, OnChangedRender(nameof(OnActivatedChanged))]
    public NetworkBool IsActivated { get; private set; }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetActivated(NetworkBool value)
    {
        IsActivated = value;
    }

    void OnActivatedChanged() { }

    public override void FixedUpdateNetwork()
    {
        float currentX = transform.localEulerAngles.x;
        float targetX = IsActivated ? _openAngle : _closeAngle;

        float newX = Mathf.MoveTowardsAngle(currentX, targetX, _speed * Runner.DeltaTime);

        var euler = transform.localEulerAngles;
        euler.x = newX;
        transform.localEulerAngles = euler;
    }
}