using UnityEngine;

public class Destroyable : MonoBehaviour, IDamageable
{
    public void TakeDamage(AttackType attackedOf)
    {
        Destroy(gameObject);
    }
}
