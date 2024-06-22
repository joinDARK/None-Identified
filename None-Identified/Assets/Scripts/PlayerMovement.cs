using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;
    private Camera _camera;
    private Vector3 _moveDirection;
    private Vector3 _velocity;
    float rotationX;
    bool isGrounded;
    [SerializeField] private Transform _playerCheckGroundTransform;
    [SerializeField] private LayerMask _playerGroundMask;

    [Header("Параметры Игрока | Player Settings")]
    [SerializeField] float _playerSpeed = 1.0f;
    [SerializeField] float _playerRunSpeed = 2.0f;
    [SerializeField] float _playerCrouchSpeed = 0.5f;
    [SerializeField] float _playerJumpPower = 1.0f;
    [SerializeField] float _playerGravity = -9.81f;
    [SerializeField] float _playerCheckRadiusSphere = 0.2f;
    [Range(0, 10)]
    [SerializeField] float _playerSensitivity = 1f;
    

    private void Start()
    {
        _controller = GetComponentInChildren<CharacterController>();
        _camera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MovePlayer();
        GravityPlayer();
        RotatePlayer();
        
    }

    private void RotatePlayer() { //Поворот персонажа
        float mouseX = Input.GetAxis("Mouse X") * _playerSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _playerSensitivity * 100f * Time.deltaTime;

        rotationX -= mouseY;

        rotationX = Mathf.Clamp(rotationX, -90, 90f);

        _camera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        transform.Rotate(Vector3.up * mouseX);
    }

    private void MovePlayer() { //Управление персонажа
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        _moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        if (Input.GetKey(KeyCode.LeftShift) && (horizontalInput != 0 || verticalInput != 0) && isGrounded) { //Ускорение персонажа
            _controller.Move(_moveDirection * _playerRunSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftControl) && (horizontalInput != 0 || verticalInput != 0) && isGrounded) {
            _controller.Move(_moveDirection * _playerCrouchSpeed * Time.deltaTime);
        }
        else {
            _controller.Move(_moveDirection * _playerSpeed * Time.deltaTime);
        }
    }

    private void GravityPlayer() { //Функция подсчета гравитации и прыжка
        isGrounded = Physics.CheckSphere(_playerCheckGroundTransform.position, _playerCheckRadiusSphere, _playerGroundMask);

        if (isGrounded && _velocity.y < 0) {
            _velocity.y = -2f;
        }

        _velocity.y += Time.deltaTime * _playerGravity;

        if (Input.GetButtonDown("Jump") && isGrounded ) {
            _velocity.y = Mathf.Sqrt(_playerJumpPower * -2f * _playerGravity);
        }

        _controller.Move(_velocity * Time.deltaTime);
    }

    private void OnDrawGizmos() { //Рисует сферу CheckGround
        if (isGrounded) {
            Gizmos.color = Color.green;
        }
        else {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawSphere(_playerCheckGroundTransform.position, _playerCheckRadiusSphere);
    }
}
