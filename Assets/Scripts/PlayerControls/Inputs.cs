using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour
{
    PlayerController _playerController;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    public void OnMove(InputValue value)
    {
        _playerController.move = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        _playerController.look = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        _playerController.jump = value.isPressed;
    }

    public void OnSprint(InputValue value)
    {
        _playerController.sprinting = value.isPressed;
    }

    public void OnInteract(InputValue value)
    {
        _playerController.interact = value.isPressed;
    }

    public void OnInventory(InputValue value)
    {
        //inventory
    }
}
