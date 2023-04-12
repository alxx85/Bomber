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
    private int _speed;

    public Vector3 Direction => _rotateDirection;

    public event Action<Boost> PickUpBooster;

    private void Awake()
    {
        _setting = GameSettings.Instance;
        _controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _setting.ChangedPlayerProperties += InitPlayer;
    }

    private void OnDisable()
    {
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