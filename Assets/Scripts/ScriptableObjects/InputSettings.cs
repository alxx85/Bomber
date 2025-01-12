using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInput",menuName = "Properties/PlayerInput")]
public class InputSettings : ScriptableObject
{
    [SerializeField] private KeyCode _leftKey;
    [SerializeField] private KeyCode _rightKey;
    [SerializeField] private KeyCode _forwardKey;
    [SerializeField] private KeyCode _backKey;
    [SerializeField] private KeyCode _setBombKey;
    [SerializeField] private KeyCode _controlBombKey;

    public KeyCode LeftKey => _leftKey;
    public KeyCode RightKey => _rightKey;
    public KeyCode ForwardKey => _forwardKey;
    public KeyCode BackKey => _backKey;
    public KeyCode SetBombKey => _setBombKey;
    public KeyCode ControlBombKey => _controlBombKey;

    public void ChangeKey(ButtonKey button, KeyCode key)
    {
        switch (button)
        {
            case ButtonKey.Left:
                _leftKey = key;
                break;
            case ButtonKey.Right:
                _rightKey = key;
                break;
            case ButtonKey.Forward:
                _forwardKey = key;
                break;
            case ButtonKey.Back:
                _backKey = key;
                break;
            case ButtonKey.SetBomb:
                _setBombKey = key;
                break;
            case ButtonKey.ControlBomb:
                _controlBombKey = key;
                break;
        }
    }
}

public enum ButtonKey
{
    Left,
    Right,
    Forward,
    Back,
    SetBomb,
    ControlBomb
}
