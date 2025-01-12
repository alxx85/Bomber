using System;
using UnityEngine;

public class PlayerInput : Inputs
{
    private GameSettings _settings;
    private Vector3 _direction = Vector3.zero;

    public event Action SetedBomb;
    public event Action KickedBomb;

    private void Start()
    {
        _settings = GameSettings.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(_settings.InputKeys.SetBombKey))
        {
            SetedBomb?.Invoke();
        }
        
        if (Input.GetKeyDown(_settings.InputKeys.ControlBombKey))
        {
            KickedBomb?.Invoke();
        }

        if (Input.GetKey(_settings.InputKeys.LeftKey))
            _direction = Vector3.left;
        else if (Input.GetKey(_settings.InputKeys.RightKey))
            _direction = Vector3.right;
        else if (Input.GetKey(_settings.InputKeys.ForwardKey))
            _direction = Vector3.forward;
        else if (Input.GetKey(_settings.InputKeys.BackKey))
            _direction = Vector3.back;
        else
            _direction = Vector3.zero;
    }

    public override Vector3 GetDirection()
    {
        return _direction;
    }
}
