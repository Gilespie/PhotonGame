using Fusion;
using UnityEngine;

public class CharacterInputController : NetworkBehaviour
{
    [Header("Buttons")]
    [SerializeField] KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] KeyCode _shootKey = KeyCode.Mouse0;
    [SerializeField] KeyCode _sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode _crouchKey = KeyCode.LeftControl;
    [SerializeField] KeyCode _interactKey = KeyCode.E;
    [SerializeField] KeyCode _ragdollKey = KeyCode.R;

    bool _isCrouchPressed = false;
    public bool IsCrouchPressed => _isCrouchPressed;

    bool _isJumpPressed = false;
    public bool IsJumpPressed => _isJumpPressed;

    bool _isSprintPressed = false;
    public bool IsSprintPressed => _isSprintPressed;

    bool _isInteractPressed = false;
    public bool IsInteractPressed => _isInteractPressed;

    bool _isShootPressed = false;
    public bool IsShootPressed => _isShootPressed;

    bool _isRagdollPressed = false;
    public bool IsRagdollPressed => _isRagdollPressed;

    Vector3 _directionPressed;
    public Vector3 DirectionPressed => _directionPressed;

    public void ArtificialUpdate()
    {
        _directionPressed = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(_crouchKey)) _isCrouchPressed = !_isCrouchPressed;
        if (Input.GetKeyDown(_sprintKey)) _isSprintPressed = !_isSprintPressed;

        if (Input.GetKeyDown(_jumpKey)) _isJumpPressed = true;
        else _isJumpPressed = false;

        if(Input.GetKeyDown(_interactKey))
        {
            _isInteractPressed = true;
        }
        else _isInteractPressed = false;

        if(Input.GetKeyDown(KeyCode.R)) _isRagdollPressed = !_isRagdollPressed;

        if (Input.GetKey(_shootKey)) _isShootPressed = true;
        else _isShootPressed = false;
    }
}