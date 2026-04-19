using UnityEngine;

public class InteractRaycast : GroundRaycast
{
    [SerializeField] float _intRadius = 0.3f;

    public override bool IsRaycasting(Vector3 direction)
    {
        _ray = new Ray(_originPoint.position, direction);

        return _isHitted = Runner.GetPhysicsScene().SphereCast(_ray.origin, _intRadius, _ray.origin + direction, out _hit, _rayDistance, _affectedLayer); ;
    }

    private new void OnDrawGizmos()
    {
        Gizmos.color = _isHitted ? Color.green : Color.red;
        Gizmos.DrawLine(_ray.origin, _ray.origin + _ray.direction * _rayDistance);
        Gizmos.DrawWireSphere(_hit.point, _intRadius);
    }
}