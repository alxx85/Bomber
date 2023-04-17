﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _stopMoveingDelay = .8f;
    [SerializeField] private LayerMask _blockedMask;
    [SerializeField] private bool _isFreeChangedDirection;

    private List<Vector3> _possibleDirections = new List<Vector3> { Vector3.left, Vector3.forward, Vector3.back, Vector3.right };
    private CharacterController _controller;
    private Animator _animator;
    private Vector3 _moveDirection = Vector3.zero;
    private float _rotationSpeed = 0.1f;
    private System.Random _getRandom = new System.Random();
    private float _blockedSearchDelay = .5f;
    private float _currentDelay;
    private bool _isBlocked;

    private void Start()
    {
        _controller = gameObject.GetComponent<CharacterController>();
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
    }

    private void GetDirections()
    {
        List<Vector3> blockedDirections = GetBlockedDirection();
        List<Vector3> directions = _possibleDirections.Except(blockedDirections).ToList();
        _controller.transform.position = GetRoundPosition();

        if (directions.Count > 0)
        {
            int directionIndex = _getRandom.Next(directions.Count());
            _moveDirection = directions[directionIndex] * -1;
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
            Debug.Log($"Attacked player on direction {hit.point - transform.TransformPoint(player.transform.position)}");
            player.GetComponent<Character>().TakeDamage();
        }

        _moveDirection = Vector3.zero;
    }

    private List<Vector3> GetBlockedDirection()
    {
        List<Vector3> blockedDirection = new List<Vector3>();
        Vector3 position = GetRoundPosition();
        position.y = .5f;

        Collider[] colliders = Physics.OverlapSphere(position, .6f, _blockedMask);

        foreach (var item in colliders)
        {
            Vector3 direction = position - item.transform.position;
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
}