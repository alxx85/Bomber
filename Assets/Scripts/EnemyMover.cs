using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyMover : MonoBehaviour
{
    
    [SerializeField] private Vector3 _moveDirection = Vector3.zero;

    private List<Vector3> _possibleDirections = new List<Vector3> { Vector3.left, Vector3.forward, Vector3.back, Vector3.right };
    private CharacterController _controller;
    private Animator _animator;
    private float _speed = 2f;
    private float _rotationSpeed = 0.1f;
    private System.Random _getRandom = new System.Random();

    private void Start()
    {
        _controller = gameObject.GetComponent<CharacterController>();
        GetDirections();
    }

    private void Update()
    {
        Rotation();
     
        if (_moveDirection != Vector3.zero)
            MoveingPlayer();
    }

    private void GetDirections()
    {
        List<Vector3> blockedDirections = GetBlockedDirection();
        List<Vector3> directions = _possibleDirections.Except(blockedDirections).ToList();
        
        foreach (var direction in directions)
        {
            Debug.Log(direction);
        }

        int directionIndex = _getRandom.Next(directions.Count());
        _moveDirection = directions[directionIndex] * -1;
    }

    private void MoveingPlayer()
    {
        _controller.Move(_moveDirection * Time.deltaTime * _speed);
    }

    private void Rotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_moveDirection), _rotationSpeed);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        GetDirections();
    }

    private List<Vector3> GetBlockedDirection()
    {
        List<Vector3> blockedDirection = new List<Vector3>();
        Vector3 position = GetRoundPosition();
        position.y = .5f;

        Collider[] colliders = Physics.OverlapSphere(position, .6f);

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