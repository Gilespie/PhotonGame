using UnityEngine;
using Fusion;

public class Player1 : NetworkBehaviour
{
    [SerializeField] float _speed = 5f;
    Vector3 _dir;

    private void Update()
    {
        //if(!HasStateAuthority)
        //    return;

        _dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    public override void FixedUpdateNetwork()
    {
        transform.position += (transform.right * _dir.x  + transform.forward * _dir.z) * _speed * Runner.DeltaTime;
    }    
}
