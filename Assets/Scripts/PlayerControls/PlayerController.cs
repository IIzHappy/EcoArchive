using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Character Input")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool interact;
    public bool sprinting;

    [Header("Player")]
    public float MoveSpeed = 4.0f;
    public float SprintSpeed = 6.0f;
    public float AccelRate = 10.0f;
    public float JumpPower = 5f;

    private float _speed;

    [Header("Camera")]
    public Camera _playerCam;
    [SerializeField] float _topClamp = 90f;
    [SerializeField] float _bottomClamp = -90f;
    [SerializeField] float _cameraSens = 1;
    float _cameraPitch;
    float _rotationVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        CameraRotation();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float targetSpeed = sprinting ? SprintSpeed : MoveSpeed;
        if (move == Vector2.zero) targetSpeed = 0.0f;

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
        _cameraPitch += look.y * -_cameraSens;
        _rotationVelocity = look.x * _cameraSens;

        _cameraPitch = Mathf.Clamp(_cameraPitch, _bottomClamp, _topClamp);

        _playerCam.transform.localRotation = Quaternion.Euler(_cameraPitch, 0.0f, 0.0f);
        transform.Rotate(Vector3.up * _rotationVelocity);
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

    }
    public void Interact()
    {

    }
}
