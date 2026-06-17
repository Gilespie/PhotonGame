using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [Header("Weapon")]
    [SerializeField] Weapon _weapon;

    [Header("Life")]
    [SerializeField] HealthSystem _health;

    [Header("Movement")]
    [SerializeField] MovementComponent _movement;

    [SerializeField] CharacterRotator _characterRotator;
    [SerializeField] GroundRaycast _groundRaycast;
    [SerializeField] InteractRaycast _interactRaycast;
    [SerializeField] CharacterAnimationController _animationController;
    [Networked] public NetworkBool _isGround { get; private set; }

    Camera _camera;
    
    //[SerializeField] CharacterMeshSelector _meshSelector;
    //int _randomMeshIndex = 0;

    public override void Spawned()
    {
        /*_randomMeshIndex = UnityEngine.Random.Range(0, _meshSelector.MecanimAnims.Length);

         _meshSelector.SelectMesh(_randomMeshIndex);
         _animationController.SetAnimator(_meshSelector.MecanimAnims[_randomMeshIndex]);*/

        if (HasInputAuthority)
        {
            _camera = Camera.main;
            _camera.GetComponent<FollowTarget>().SetTarget(this);
        }

        GameManager.Instance.AddToList(this);
    }
        
    public override void FixedUpdateNetwork()
    {
        _isGround = _groundRaycast.IsRaycasting(Vector3.down);
        _movement.SetGrounded(_isGround);
        //_animationController.SetBool(AnimParams.Air, !_isGround);

        if (!GetInput(out NetworkInputData inputs)) return;
        if (_health.IsDead) return;

        _movement.Movement(inputs.MovementInput, Runner);

        if (inputs.networkButtons.IsSet(MyButtons.Jump))
        {
            _movement.Jump(_isGround);
        }

        if (inputs.networkButtons.IsSet(MyButtons.Shoot) && _weapon.ReadyToFire && !inputs.networkButtons.IsSet(MyButtons.Sprint))
        {
            _weapon.Fire();
        }

        _movement.SetCrouch(inputs.networkButtons.IsSet(MyButtons.Crouch));
        _movement.SetSprint(inputs.networkButtons.IsSet(MyButtons.Sprint));

        _characterRotator.RotateDefault(inputs.MovementInput);
    }
}