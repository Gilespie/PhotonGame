using Fusion;
using UnityEngine;

public class Bridge : NetworkBehaviour
{
    [SerializeField] NetworkMecanimAnimator _animator;

    [Networked, OnChangedRender(nameof(OnActivatedChanged))]
    public NetworkBool IsActivated { get; private set; }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetActivated(NetworkBool value)
    {
        IsActivated = value;
    }

    void OnActivatedChanged()
    {
        _animator.Animator.SetBool(AnimParams.BridgeActivated, IsActivated);
    }
}