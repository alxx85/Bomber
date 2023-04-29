using System;
using UnityEngine;

public class PlayerInput : Inputs
{
    private GameSettings _settings;
    private KeyCode _leftButton = KeyCode.LeftArrow;
    private KeyCode _rightButton = KeyCode.RightArrow;
    private KeyCode _forwardButton = KeyCode.UpArrow;
    private KeyCode _backButton = KeyCode.DownArrow;
    private KeyCode _setBombButton = KeyCode.Space;
    private KeyCode _kickBombButton = KeyCode.E;
    private Vector3 _direction = Vector3.zero;

    public event Action SetedBomb;
    public event Action KickedBomb;

    private void Start()
    {
        _settings = GameSettings.Instance;
        InitKeys();
    }

    private void InitKeys()
    {
        _leftButton = _settings.LeftKey;
        _rightButton = _settings.RightKey;
        _forwardButton = _settings.ForwardKey;
        _backButton = _settings.BackKey;
    }

    private void Update()
    {
        if (Input.GetKeyDown(_setBombButton))
        {
            SetedBomb?.Invoke();
        }
        
        if (Input.GetKeyDown(_kickBombButton))
        {
            KickedBomb?.Invoke();
        }

        if (Input.GetKey(_leftButton))
            _direction = Vector3.left;
        else if (Input.GetKey(_rightButton))
            _direction = Vector3.right;
        else if (Input.GetKey(_forwardButton))
            _direction = Vector3.forward;
        else if (Input.GetKey(_backButton))
            _direction = Vector3.back;
        else
            _direction = Vector3.zero;
    }

    public override Vector3 GetDirection()
    {
        return _direction;
    }
}
