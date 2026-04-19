using Fusion;
using UnityEngine;

public class GroundRaycast : NetworkBehaviour
{
    [SerializeField] protected Transform _originPoint;
    [SerializeField] protected float _rayDistance = 0.45f;
    [SerializeField] protected LayerMask _affectedLayer;
    protected RaycastHit _hit;
    protected Ray _ray;
    protected bool _isHitted;

    public virtual bool IsRaycasting(Vector3 direction)
    {
        _ray = new Ray(_originPoint.position, direction);

        return _isHitted = Runner.GetPhysicsScene().Raycast(_ray.origin, _ray.direction, out _hit, _rayDistance, _affectedLayer);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = _isHitted ? Color.green : Color.red;
        Gizmos.DrawLine(_ray.origin, _ray.origin + _ray.direction * _rayDistance);
    }
}