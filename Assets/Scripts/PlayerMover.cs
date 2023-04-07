using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public const string Horizontal = "Horizontal";
    public const string Vertical = "Vertical";

    [SerializeField] private int _speed;
    [SerializeField] private float _rotateSpeed;

    private CharacterController _controller;
    private Vector3 _moveDirection;
    private Vector3 _rotateDirection;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlayerInput();

        Move();
    }

    private void PlayerInput()
    {
        _moveDirection = new Vector3(Input.GetAxisRaw(Horizontal), 0, Input.GetAxisRaw(Vertical));

        if (_moveDirection.x != 0)
            _moveDirection.z = 0;

        if (_moveDirection != Vector3.zero)
            _rotateDirection = _moveDirection;
    }

    private void Move()
    {
        _controller.Move(_moveDirection * _speed * Time.deltaTime);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_rotateDirection), _rotateSpeed);
    }
}