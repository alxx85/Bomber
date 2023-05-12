using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Movement : MonoBehaviour
{
    private const float RotationSpeed = 0.2f;
    private const int AngleMultiplier = -1;

    protected Rigidbody _rbody;
    private CapsuleCollider _collider;
    protected Vector3 _moveDirection = Vector3.zero;
    protected Vector3 _rotateDirection = Vector3.zero;

    public float CurrentRadius => _collider.radius;

    private void Awake()
    {
        _rbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
    }

    protected void Moveing(float speed)
    {
        _moveDirection.y = _rbody.velocity.y;
        _rbody.velocity = _moveDirection * speed;
    }

    protected void Rotation(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            float _angle = Vector3.SignedAngle(direction, Vector3.forward, Vector3.up);
            _rbody.MoveRotation(Quaternion.Slerp(_rbody.rotation, Quaternion.AngleAxis(_angle * AngleMultiplier, Vector3.up), RotationSpeed));
        }
    }
}
