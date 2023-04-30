using UnityEngine;

public interface IDamageable
{
    void TakeDamage(AttackType attacked);
}

public enum AttackType
{
    Bomb,
    Enemy
}