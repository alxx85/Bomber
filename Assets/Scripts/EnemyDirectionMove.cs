using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDirectionMove : MonoBehaviour
{
    [SerializeField] private Vector3 _moveDirection = Vector3.zero;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _stopMoveingDelay = .1f;
    [SerializeField] private LayerMask _blockedMask;
    [SerializeField] private bool _canChangingDirection;

    private List<Vector3> _possibleDirections = new List<Vector3> { Vector3.left, Vector3.forward, Vector3.back, Vector3.right };
    private CharacterController _controller;
    private Animator _animator;
    private float _rotationSpeed = 0.1f;
    private System.Random _getRandom = new System.Random();
    private float _blockedSearchDelay = .5f;
    private float _currentDelay;
    private bool _isBlocked;
    private Vector3 oldPosition;
    private bool _canChangeDirection;

    private void Start()
    {
        _controller = gameObject.GetComponent<CharacterController>();
        GetDirections();
        _currentDelay = 0;
        _isBlocked = false;
    }

    private void Update()
    {
        Rotation();

        if (_moveDirection != Vector3.zero)
        {
            MoveingPlayer();
            _currentDelay = 0;
        }
        else
        {
            _currentDelay += Time.deltaTime;

            if (_isBlocked)
            {
                if (_currentDelay >= _blockedSearchDelay)
                {
                    GetDirections();
                }
            }
            else
            {
                if (_currentDelay >= _stopMoveingDelay)
                {
                    GetDirections();
                }
            }
        }

        if (_canChangingDirection)
        {
            if (oldPosition != GetRoundPosition())
            {
                _canChangeDirection = false;
                oldPosition = GetRoundPosition();
            }

            if (_canChangeDirection == false)
            {
                Vector3 approximatePosition = GetApproximatePosition();
                float approximate = .5f - _controller.radius;

                if (Mathf.Abs(approximatePosition.x) < approximate && Mathf.Abs(approximatePosition.z) < approximate)
                {
                    if (GetRoundPosition().x % 2 == 0 && GetRoundPosition().z % 2 == 0)
                    {
                        _moveDirection = Vector3.zero;
                        _canChangeDirection = true;
                    }
                }
            }
        }
    }

    private void GetDirections()
    {
        List<Vector3> blockedDirections = GetBlockedDirection();
        List<Vector3> directions = _possibleDirections.Except(blockedDirections).ToList();
        _controller.transform.position = GetRoundPosition();

        if (directions.Count > 0)
        {
            int directionIndex = _getRandom.Next(directions.Count());
            _moveDirection = directions[directionIndex];
        }
        else
        {
            _isBlocked = true;
        }
    }

    private void MoveingPlayer()
    {
        _controller.Move(_moveDirection * Time.deltaTime * _speed);
    }

    private void Rotation()
    {
        if (_moveDirection != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_moveDirection), _rotationSpeed);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.TryGetComponent(out PlayerAttacks player))
        {
            Debug.Log("Attack");
            player.GetComponent<Character>().TakeDamage();
        }

        _moveDirection = Vector3.zero;
        //GetDirections();
    }

    private List<Vector3> GetBlockedDirection()
    {
        List<Vector3> blockedDirection = new List<Vector3>();
        Vector3 position = GetRoundPosition();
        position.y = .5f;

        Collider[] colliders = Physics.OverlapSphere(position, .6f, _blockedMask);

        foreach (var item in colliders)
        {
            Vector3 direction = item.transform.position - position;
            blockedDirection.Add(direction);
        }

        return blockedDirection;
    }

    private Vector3 GetRoundPosition()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.RoundToInt(currentPosition.x);
        currentPosition.y = 0f;
        currentPosition.z = Mathf.RoundToInt(currentPosition.z);
        return currentPosition;
    }

    private Vector3 GetApproximatePosition()
    {
        float roundPositionX = transform.position.x - GetRoundPosition().x;
        float roundPositionZ = transform.position.z - GetRoundPosition().z;

        if (Mathf.Abs(roundPositionX) > 0.6f)
            roundPositionX -= 1;

        if (Mathf.Abs(roundPositionZ) > 0.6f)
            roundPositionZ -= 1;

        return new Vector3(roundPositionX, 0, roundPositionZ);
    }
}
