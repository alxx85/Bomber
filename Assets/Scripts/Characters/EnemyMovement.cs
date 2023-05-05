using UnityEngine;

public class EnemyMovement : Movement
{
    [SerializeField] private EnemyInput _input;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _stopMoveingDelay = .1f;
    [SerializeField] private bool _canChangingDirection;

    private float _blockedSearchDelay = .5f;
    private float _unblockedDelay = .8f;
    private float _currentDelay;
    private bool _isBlocked = true;
    private Vector3 oldPosition;
    private bool _changingDirection;

    private void Start()
    {
        _currentDelay = _blockedSearchDelay;
        _moveDirection = _input.GetDirection();
    }

    private void FixedUpdate()
    {
        Rotation(_moveDirection);

        if (_moveDirection != Vector3.zero)
        {
            Moveing(_speed);
            _currentDelay = _stopMoveingDelay;
        }
        else
        {
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

        if (_canChangingDirection)
        {
            ChangingDirection();
        }
    }

    protected void ChangingDirection()
    {
        if (oldPosition != _input.GetRoundPosition(_rbody.position))
        {
            _changingDirection = false;
            oldPosition = _input.GetRoundPosition(_rbody.position);
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
                }
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        _moveDirection = Vector3.zero;
        _rbody.velocity = new Vector3(0f, _rbody.velocity.y, 0f);
        Debug.Log($"{gameObject.name} - {collision.gameObject.name}");
        _rbody.position = _input.GetRoundPosition(_rbody.position);

        if (collision.collider.TryGetComponent(out PlayerAttacks player))
        {
            Debug.Log("Attack");
            player.GetComponent<Characters>().TakeDamage(AttackType.Enemy);
        }
    }
}
