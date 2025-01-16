using UnityEngine;

public class EnemyMovement : Movement
{
    private const string MOVE = "Move";

    [SerializeField] private EnemyInput _input;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _stopMoveingDelay = .1f;
    [SerializeField] private bool _canChangingDirection;
    [SerializeField] private int _minChangingDirectionDistance = 2;
    [SerializeField] private AudioSource _moveSFX;
    [SerializeField] private SoundType _soundType = SoundType.Characters;

    private GameSettings _setting;
    private float _blockedSearchDelay = .5f;
    private float _unblockedDelay = .8f;
    private float _currentDelay;
    private bool _isBlocked = true;
    private Vector3 oldPosition;
    private bool _changingDirection;
    private int _currentDistance = 0;
    private bool _isMoveing;

    private void OnEnable()
    {
        _setting = GameSettings.Instance;
        _setting.ChangedVolume += OnChangedVolume;
        _setting.ChangedPause += OnChangedPause;
        OnChangedVolume();
    }

    private void OnDisable()
    {
        _setting.ChangedVolume -= OnChangedVolume;
        _setting.ChangedPause -= OnChangedPause;
    }

    private void Start()
    {
        _currentDelay = _blockedSearchDelay;
        _moveDirection = _input.GetDirection();
    }

    private void FixedUpdate()
    {
        if (_canChangingDirection)
            ChangingDirection();

        Rotation(_moveDirection);

        if (_moveDirection != Vector3.zero)
        {
            _rotateDirection = _moveDirection;
            Moveing(_speed);
            _animator.SetBool(MOVE, true);
            _currentDelay = _stopMoveingDelay;

            if (_isMoveing == false)
            {
                _isMoveing = true;
                _moveSFX.Play();
            }
        }
        else
        {
            _animator.SetBool(MOVE, false);
            
            if (_isMoveing)
            {
                _isMoveing = false;
                _moveSFX.Stop();
            }
            _currentDelay -= Time.deltaTime;

            if (_currentDelay <= 0 && _isBlocked)
            {
                Vector3 clearDirection = _input.GetDirection();

                if (clearDirection != Vector3.zero)
                {
                    _currentDelay = _unblockedDelay;
                    _isBlocked = false;
                }
            }
            else if (_currentDelay <= 0)
            {
                _moveDirection = _input.GetDirection();
            }
        }
    }

    protected void ChangingDirection()
    {
        if (oldPosition != _input.GetRoundPosition(_rbody.position))
        {
            if (_currentDistance >= _minChangingDirectionDistance)
            {
                _changingDirection = false;
                //_moveDirection = Vector3.zero;
                //_currentDistance = 0;
            }
            
            oldPosition = _input.GetRoundPosition(_rbody.position);
            _currentDistance++;
        }

        if (_changingDirection == false)
        {
            Vector3 approximatePosition = _input.GetApproximatePosition(_rbody.position);
            float approximate = .5f - CurrentRadius;

            if (Mathf.Abs(approximatePosition.x) < approximate && Mathf.Abs(approximatePosition.z) < approximate)
            {
                if (_input.GetRoundPosition(_rbody.position).x % 2 == 0 && _input.GetRoundPosition(_rbody.position).z % 2 == 0)
                {
                    _rbody.position = _input.GetRoundPosition(_rbody.position);
                    _moveDirection = _input.GetDirection();
                    _changingDirection = true;
                    oldPosition = _input.GetRoundPosition(_rbody.position);
                    _currentDistance = 0;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _moveDirection = Vector3.zero;
        _currentDistance = 0;
        _rbody.linearVelocity = new Vector3(0f, _rbody.linearVelocity.y, 0f);
        _rbody.position = _input.GetRoundPosition(_rbody.position);

        if (collision.collider.TryGetComponent(out PlayerAttacks player))
        {
            Vector3 contactPosition = _rbody.position - _input.GetRoundPosition(player.transform.position);
            
            if (contactPosition != _rotateDirection)
                player.GetComponent<Characters>().TakeDamage(AttackType.Enemy);
        }
    }

    private void OnChangedVolume()
    {
            _moveSFX.volume = _setting.GetVolume(_soundType);
    }

    private void OnChangedPause(bool pause)
    {
        //if (_moveSFX.isPlaying)
        //{
        if (pause)
            _moveSFX.Pause();
        else
            _moveSFX.UnPause();
        //}
    }
}
