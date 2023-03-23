using UnityEngine;
using UnityEngine.Assertions;
using System;
using UnityEngine.InputSystem;

[CreateAssetMenu]
public class InputManager : Manager
{
    private PlayerInputActions playerInputActions;

    public event EventHandler<float> OnPause;
    public event EventHandler<float> OnCollectStarted;
    public event EventHandler<float> OnCollectPerformed;

    public override void OnManualEnable()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Pause.performed += Pause_performed;
        playerInputActions.Player.Collect.started += Collect_started;
        playerInputActions.Player.Collect.performed += Collect_performed;
    }

    public override void OnManualDisable()
    {
        playerInputActions.Player.Pause.performed -= Pause_performed;
        playerInputActions.Player.Collect.started -= Collect_started;
        playerInputActions.Player.Collect.performed -= Collect_performed;
        playerInputActions.Dispose();
        playerInputActions = null;
    }

    private void Pause_performed(InputAction.CallbackContext context)
    {
        OnPause?.Invoke(this, Time.time);
    }

    private void Collect_started(InputAction.CallbackContext context)
    {
        // TODO.
    }

    private void Collect_performed(InputAction.CallbackContext context)
    {
        // TODO.
    }

    public Vector3 GetMovement()
    {
        Assert.IsNotNull(playerInputActions);
        var move = playerInputActions.Player.Move.ReadValue<Vector2>();
        return Vector3.ClampMagnitude(new Vector3(move.x, 0f, move.y), 1f);
    }

    public Vector2 GetLookAround()
    {
        return playerInputActions.Player.LookAround.ReadValue<Vector2>();
    }

    public bool GetRun()
    {
        return playerInputActions.Player.Run.IsPressed();
    }

    public bool GetCollect()
    {
        return playerInputActions.Player.Collect.IsPressed();
    }
}
