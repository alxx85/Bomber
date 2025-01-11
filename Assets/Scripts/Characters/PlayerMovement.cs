using UnityEngine;

[RequireComponent (typeof(PlayerSFX))]
public class PlayerMovement : Movement
{
    private const string MOVE = "IsMoving";
    private const string SPEED = "Speed";

    [SerializeField] private Inputs _input;

    private GameSettings _settings;
    private PlayerSFX _audio;
    private Camera_Controller _camera;
    private float _audioModifiere = 0.25f;
    private bool _isMoveing;

    public Vector3 Direction => _rotateDirection;

    protected override void Awake()
    {
        base.Awake();
        _audio = GetComponent<PlayerSFX>();
    }

    private void Start()
    {
        _settings = GameSettings.Instance;
        //_normalSpeed = _settings.Speed;
        Camera.main.GetComponent<Camera_Controller>().InitPlayer(this);
        _animator.SetFloat(SPEED, _settings.SpeedLevel);
    }

    private void FixedUpdate()
    {
        Rotation(_rotateDirection);
        _moveDirection = _input.GetDirection();

        if (_moveDirection != Vector3.zero)
        {
            _rotateDirection = _moveDirection;
            Moveing(_settings.Speed);
            float animationSpeed = _settings.SpeedLevel;
            _animator.SetFloat(SPEED, animationSpeed + 1);
            _animator.SetBool(MOVE, true);

            if (_isMoveing == false)
            {
                _isMoveing = true;
                _audio.Play(AudioName.Walk);
            }
        }
        else
        {
            _animator.SetBool(MOVE, false);

            if (_isMoveing)
            {
                _audio.Stop();
                _isMoveing = false;
            }
        }
    }
}
