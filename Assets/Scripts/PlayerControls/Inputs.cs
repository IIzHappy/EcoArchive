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
        _playerController.Jump();
    }

    public void OnSprint(InputValue value)
    {
        _playerController._sprintDown = value.isPressed;
        _playerController.UpdateWalkState();
    }

    public void OnSlowWalk(InputValue value)
    {
        _playerController._slowWalkDown = value.isPressed;
        _playerController.UpdateWalkState();
    }

    public void OnInteract(InputValue value)
    {
        _playerController.Interact();
    }

    public void OnCollection(InputValue value)
    {
        _playerController.CollectionMenu();
    }
}
