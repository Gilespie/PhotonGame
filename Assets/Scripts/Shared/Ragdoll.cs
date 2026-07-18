using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class Ragdoll : NetworkBehaviour
{
    [SerializeField] Rigidbody[] _ragdollRBs;
    [SerializeField] NetworkMecanimAnimator _animatorNetwork;
    [SerializeField] Animator _animator;
    [SerializeField] Collider _collider;
    [SerializeField] NetworkRigidbody3D _mainRB;

    public void DisableRagdoll()
    {
        Debug.Log($"DisableRagdoll {gameObject.name}");
        foreach (var rb in _ragdollRBs)
        {
            rb.isKinematic = true;
        }

        _animator.enabled = true;
        _animatorNetwork.enabled = true;
        _animator.Rebind();
        _animator.Update(0f);
        _collider.enabled = true;
        _mainRB.Rigidbody.isKinematic = false;
    }

    public void ActivateRagdoll()
    {
        Debug.Log($"ActivateRagdoll {gameObject.name}");

        _animator.enabled = false;
        _animatorNetwork.enabled = false;   
        _collider.enabled = false;
        _mainRB.Rigidbody.isKinematic = true;

        foreach (var rb in _ragdollRBs)
        {
            rb.isKinematic = false;
        } 
    }
}