using UnityEngine;

public class PlayerMovement : Movement
{
    private const string MOVE = "IsMoving";
    private const string SPEED = "Speed";

    [SerializeField] private Inputs _input;

    private GameSettings _settings;
    private Camera_Controller _camera;
    private float _normalSpeed;

    public Vector3 Direction => _rotateDirection;

    private void Start()
    {
        _settings = GameSettings.Instance;
        _normalSpeed = _settings.Speed;
        Camera.main.GetComponent<Camera_Controller>().InitPlayer(this);
        _animator.SetFloat(SPEED, _settings.Speed - _normalSpeed);
    }

    private void FixedUpdate()
    {
        Rotation(_rotateDirection);
        _moveDirection = _input.GetDirection();

        if (_moveDirection != Vector3.zero)
        {
            _rotateDirection = _moveDirection;
            Moveing(_settings.Speed);
            float animationSpeed = _settings.Speed - _normalSpeed;
            _animator.SetFloat(SPEED, animationSpeed + 1);
            _animator.SetBool(MOVE, true);
        }
        else
        {
            _animator.SetBool(MOVE, false);
        }
    }
}
