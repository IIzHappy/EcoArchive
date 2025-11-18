using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Character Input")]
    public Vector2 move;
    public Vector2 look;
    [SerializeField] int walkState = 0;
    //0-walk
    //1-sprint
    //2-slow walk
    public bool _sprintDown;
    public bool _slowWalkDown;

    [Header("Player")]
    public int[] _moveSpeed = new int[3];
    //0-walk
    //1-sprint
    //2-slow walk
    public float AccelRate = 10.0f;
    public float JumpPower = 5f;
    bool _isGrounded;
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundedThreshold;

    private float _speed;

    [Header("Camera")]
    public Camera _playerCam;
    [SerializeField] float _topClamp = 90f;
    [SerializeField] float _bottomClamp = -90f;
    [SerializeField] float _cameraSens = 1;
    float _cameraPitch;
    float _rotation;

    bool _canRotate = true;

    [Header("Collection Menu")]
    public GameObject _collection;
    bool _collectionOpen;

    [Header("Interactables")]
    [SerializeField] GameObject _crosshair;
    bool _interactable;
    [SerializeField] float _interactDistance = 3;
    GameObject _curInteractable;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        _collection.SetActive(false);
    }

    void LateUpdate()
    {
        CameraRotation();
    }

    void Update()
    {
        Move();
        if (!_isGrounded) GroundedCheck();
        InteractCheck();
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
        float targetSpeed = 0;
        if (move != Vector2.zero)
        {
            targetSpeed = _moveSpeed[walkState];
        }

        float currentHorizontalSpeed = _speed;

        float speedOffset = 0.1f;
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * AccelRate);
            //_speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        Vector3 inputDirection = transform.right * move.x + transform.forward * move.y;
        rb.MovePosition(inputDirection.normalized * (Time.deltaTime * _speed) + rb.position);
    }

    private void CameraRotation()
    {
        if (_canRotate)
        {
            _cameraPitch += look.y * -_cameraSens;
            _rotation += look.x * _cameraSens;
        }

        _cameraPitch = Mathf.Clamp(_cameraPitch, _bottomClamp, _topClamp);

        _playerCam.transform.localRotation = Quaternion.Euler(_cameraPitch, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, _rotation, 0f);
    }
    public Vector3 GetLookDir()
    {
        return _playerCam.transform.forward;
    }
    public Transform GetEyePos()
    {
        return _playerCam.transform;
    }

    public void Jump()
    {
        if (_isGrounded) rb.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
        _isGrounded = false;
    }
    void GroundedCheck()
    {
        _isGrounded = Physics.Raycast(_groundCheck.position, Vector3.down, _groundedThreshold, LayerMask.GetMask("Ground"));
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

    public void CollectionMenu()
    {
        _collectionOpen = !_collectionOpen;
        _collection.SetActive(_collectionOpen);
        Cursor.lockState = _collectionOpen ? CursorLockMode.None : CursorLockMode.Locked;
        _canRotate = !_collectionOpen;
    }
}
