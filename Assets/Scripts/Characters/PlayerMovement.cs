using UnityEngine;

public class PlayerMovement : Movement
{
    [SerializeField] private Inputs _input;

    private GameSettings _settings;
    private Camera_Controller _camera;

    public Vector3 Direction => _rotateDirection;

    private void Start()
    {
        _settings = GameSettings.Instance;
        Camera.main.GetComponent<Camera_Controller>().InitPlayer(this);
    }

    private void FixedUpdate()
    {
        Rotation(_rotateDirection);
        _moveDirection = _input.GetDirection();

        if (_moveDirection != Vector3.zero)
        {
            _rotateDirection = _moveDirection;
            Moveing(_settings.Speed);
        }
    }
}
