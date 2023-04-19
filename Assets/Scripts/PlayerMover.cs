using System;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public const string Horizontal = "Horizontal";
    public const string Vertical = "Vertical";

    [SerializeField] private float _rotateSpeed;

    private CharacterController _controller;
    private GameSettings _setting;
    private Vector3 _moveDirection;
    private Vector3 _rotateDirection = Vector3.forward;
    private float _speed;
    private KeyCode _leftButton = KeyCode.LeftArrow;
    private KeyCode _rightButton = KeyCode.RightArrow;
    private KeyCode _forwardButton = KeyCode.UpArrow;
    private KeyCode _backButton = KeyCode.DownArrow;
    private Camera_Controller _camera;

    public Vector3 Direction => _rotateDirection;

    public event Action<Boost> PickUpBooster;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main.GetComponent<Camera_Controller>();

}

private void OnEnable()
    {
        _setting = GameSettings.Instance;
        _setting.InitPlayer(this);
        _setting.ChangedPlayerProperties += InitPlayer;
        _camera.InitPlayer(this);
    }

    private void OnDisable()
    {
        if (_setting != null)
            _setting.ChangedPlayerProperties -= InitPlayer;
    }

    private void Start()
    {
        InitPlayer();
    }

    private void Update()
    {
        PlayerInput();

        Move();
    }

    public void PickUp(Boost boost)
    {
        PickUpBooster?.Invoke(boost);
    }

    private void InitPlayer()
    {
        _speed = _setting.Speed;
        _leftButton = _setting.LeftKey;
        _rightButton = _setting.RightKey;
        _forwardButton = _setting.ForwardKey;
        _backButton = _setting.BackKey;
    }

    private void PlayerInput()
    {
        //_moveDirection = new Vector3(Input.GetAxisRaw(Horizontal), 0, Input.GetAxisRaw(Vertical));
        if (Input.GetKey(_leftButton))
            _moveDirection = Vector3.left;
        else if (Input.GetKey(_rightButton))
            _moveDirection = Vector3.right;
        else if (Input.GetKey(_forwardButton))
            _moveDirection = Vector3.forward;
        else if (Input.GetKey(_backButton))
            _moveDirection = Vector3.back;
        else
            _moveDirection = Vector3.zero;
        //if (_moveDirection.x != 0)
        //    _moveDirection.z = 0;

        if (_moveDirection != Vector3.zero)
            _rotateDirection = _moveDirection;
    }

    private void Move()
    {
        if (transform.position.y != 0)
            _moveDirection.y = transform.position.y * -1;

        _controller.Move(_moveDirection * _speed * Time.deltaTime);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_rotateDirection), _rotateSpeed);
    }
}