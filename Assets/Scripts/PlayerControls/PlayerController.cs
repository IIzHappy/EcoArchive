using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController _playerController;
    public bool _canControl = true;

    [SerializeField] PlayerRotateCam _rotateCam;

    [Header("Player")]
    int walkState = 0;
    public bool _sprintDown;
    public bool _slowWalkDown;
    public int[] _moveSpeed = new int[3];
    //0-walk
    //1-sprint
    //2-slow walk

    public float _gravity = 9.81f;
    Vector3 _velocity;

    bool _isGrounded;
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundedThreshold;

    [SerializeField] LayerMask _groundMask;

    public float _jumpHeight = 3f;

    PlayerSound _playerSound;
    bool _isMoving;

    [Header("Camera")]
    public Camera _playerCam;

    [Header("Interactables")]
    [SerializeField] GameObject _crosshair;
    bool _interactable;
    [SerializeField] float _interactDistance = 3;
    GameObject _curInteractable;

    void Start()
    {
        _playerController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        _rotateCam = GetComponentInChildren<PlayerRotateCam>();
        PlayerInputs.Instance.PauseGame(false);
        _playerSound = GetComponent<PlayerSound>();
    }

    void Update()
    {
        if (!_canControl) return;
        _rotateCam.RotateCam();
        Move();
    }

    public void UpdateWalkState()
    {
        if (_sprintDown)
        {
            walkState = 1;
            return;
        }
        else if (_slowWalkDown)
        {
            walkState = 2;
            return;
        }
        walkState = 0;
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 move = transform.right * x + transform.forward * z;

        _playerController.Move(move * _moveSpeed[walkState] *Time.deltaTime);

        _velocity.y += _gravity * Time.deltaTime;
        _playerController.Move(_velocity * Time.deltaTime);

        _isGrounded = Physics.CheckSphere(_groundCheck.position,_groundedThreshold, _groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -1f;
        }
    }

    public void Jump()
    {
        _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
    }

    public Vector3 GetLookDir()
    {
        return _playerCam.transform.forward;
    }
    public Transform GetEyePos()
    {
        return _playerCam.transform;
    }

    public void Interact()
    {
        if (_interactable)
        {
            Collectables item = _curInteractable.GetComponent<Collectables>();
            if (item._bone != null)
            {
                Collection.Instance.AddBone(item._bone);
                Destroy(_curInteractable);
                InteractCheck();
            }
        }
    }
    void InteractCheck()
    {
        Ray ray = new Ray(GetEyePos().position, GetLookDir());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _interactDistance))
        {
            if (hit.collider.gameObject.tag == "Bones")
            {
                _interactable = true;
                _curInteractable = hit.collider.gameObject;
                _crosshair.SetActive(true);
                return;
            }
        }
        _interactable = false;
        _curInteractable = null;
        _crosshair.SetActive(false);
    }
}
