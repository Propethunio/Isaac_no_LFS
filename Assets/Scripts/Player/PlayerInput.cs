using UnityEngine;

public class PlayerInput : MonoBehaviour {

    PlayerInputActions playerInputActions;

    void Awake() {
        playerInputActions = new PlayerInputActions();
        EnablePlayerInput();
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }

    public Vector2 GetShootingVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Shoot.ReadValue<Vector2>();
        return inputVector.normalized;
    }

    public void EnablePlayerInput() {
        playerInputActions.Player.Enable();
    }

    public void DisablePlayerInput() {
        playerInputActions.Player.Disable();
    }
}