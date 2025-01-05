using UnityEngine;

public class PlayerMovement : Movement
{
    private const string MOVE = "IsMoving";
    private const string SPEED = "Speed";

    [SerializeField] private Inputs _input;

    private GameSettings _settings;
    private Camera_Controller _camera;

    public Vector3 Direction => _rotateDirection;

    private void Start()
    {
        _settings = GameSettings.Instance;
        Camera.main.GetComponent<Camera_Controller>().InitPlayer(this);
        _animator.SetFloat(SPEED, _settings.Speed);
    }

    private void FixedUpdate()
    {
        Rotation(_rotateDirection);
        _moveDirection = _input.GetDirection();

        if (_moveDirection != Vector3.zero)
        {
            _rotateDirection = _moveDirection;
            Moveing(_settings.Speed);
            _animator.SetFloat(SPEED, _settings.Speed);
            _animator.SetBool(MOVE, true);
        }
        else
        {
            _animator.SetBool(MOVE, false);
        }
    }
}
