using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public static PlayerInputs Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [SerializeField] PlayerController _playerController;
    [SerializeField] UIManager _uiManager;

    public bool _playerActive;

    public void UpdateUIManager(UIManager uiManager)
    {
        _uiManager = uiManager;
    }

    public void OnMove(InputValue value)
    {
        if (!_playerActive)
        {
            _playerController.move = new Vector2(0, 0);
        }
        else
        {
            _playerController.move = value.Get<Vector2>();
        }
    }

    public void OnLook(InputValue value)
    {
        if (!_playerActive)
        {
            _playerController.look = new Vector2(0, 0);
        }
        else
        {
            _playerController.look = value.Get<Vector2>();
        }
    }

    public void OnJump(InputValue value)
    {
        if (!_playerActive) return;
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
        if (!_playerActive) return;
        _playerController.Interact();
    }


    public void OnEscape(InputValue value)
    {
        _uiManager.EscapePressed();
    }
    public void OnCollection(InputValue value)
    {
        _uiManager.CollectionPressed();
    }
}
