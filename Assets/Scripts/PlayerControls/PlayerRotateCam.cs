using UnityEngine;

public class PlayerRotateCam : MonoBehaviour
{
    public Transform _playerBody;

    public float _mouseSensX = 100f;
    public float _mouseSensY = 100f;

    float _xRotation = 0;

    [SerializeField] float _topClamp = 90f;
    [SerializeField] float _bottomClamp = -90f;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        RotateCam();
    }

    void RotateCam()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensX * 100 * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensY * 100 * Time.deltaTime;

        _xRotation = Mathf.Clamp(_xRotation - mouseY, _bottomClamp, _topClamp);

        transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        _playerBody.Rotate(Vector3.up * mouseX);
    }
}
