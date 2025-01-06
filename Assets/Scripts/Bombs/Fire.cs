using UnityEngine;

public class Fire : MonoBehaviour
{
    private const float FireDelay = .55f;
    private Vector3 _direction;

    public Vector3 Direction => _direction;

    private void Start()
    {
        Destroy(gameObject, FireDelay);
    }

    public void Init(Vector3 direction)
    {
        _direction = direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(AttackType.Bomb);
        }
    }
}
