using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector3 MovementInput;
    public NetworkBool IsFirePressed;
    public NetworkBool IsJumpPressed;
    public NetworkBool IsSprintPressed;
    public NetworkBool IsCrouchPressed;
    public NetworkBool IsInteractPressed;
    public NetworkBool IsPausePressed;

    public NetworkButtons networkButtons;
}

enum MyButtons
{
    Jump,
    Shoot,
    Sprint,
    Crouch,
    Interact,
    Pause
}