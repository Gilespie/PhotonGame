using UnityEngine;

public class LocalInputs : MonoBehaviour
{
    private NetworkInputData _networkInputData;

    private bool _isJumpPressed;
    private bool _isFirePressed;
    private bool _isInteractPressed;
    private bool _isCrouchPressed;
    private bool _isSprintPressed;
    private bool _isPausePressed;

    [Header("Keys")]
    [SerializeField] KeyCode _jumpKey = KeyCode.W;
    [SerializeField] KeyCode _shootKey = KeyCode.Mouse0;
    [SerializeField] KeyCode _sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode _crouchKey = KeyCode.LeftControl;
    [SerializeField] KeyCode _interactKey = KeyCode.E;
    [SerializeField] KeyCode _pauseKey = KeyCode.Escape; 

    void Update()
    {
        _isJumpPressed |= Input.GetKeyDown(_jumpKey);
        _isFirePressed |= Input.GetKey(_shootKey);
        _isInteractPressed |= Input.GetKeyDown(_interactKey);
        _isCrouchPressed |= Input.GetKey(_crouchKey);
        _isSprintPressed |= Input.GetKey(_sprintKey);
        _isPausePressed |= Input.GetKeyDown(_pauseKey);
    }

    public NetworkInputData GetLocalInputs()
    {
        _networkInputData = new NetworkInputData();

        _networkInputData.MovementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        _networkInputData.networkButtons.Set(MyButtons.Interact, _isInteractPressed);
        _isInteractPressed = false;

        _networkInputData.networkButtons.Set(MyButtons.Crouch, _isCrouchPressed);
        _isCrouchPressed = false;

        _networkInputData.networkButtons.Set(MyButtons.Sprint, _isSprintPressed);
        _isSprintPressed = false;

        _networkInputData.networkButtons.Set(MyButtons.Jump, _isJumpPressed);
        _isJumpPressed = false;

        _networkInputData.networkButtons.Set(MyButtons.Shoot, _isFirePressed);
        _isFirePressed = false;

        _networkInputData.networkButtons.Set(MyButtons.Pause, _isPausePressed);
        _isPausePressed = false;

        return _networkInputData;
    }
}