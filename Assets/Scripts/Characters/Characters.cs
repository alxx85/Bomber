using System;
using UnityEngine;

public abstract class Characters : MonoBehaviour, IDamageable
{
    [SerializeField] protected int _health;
    [SerializeField] private float _takeDamageCooldown;

    private float _currentDelay;

    public event Action<Characters> Dying;

    private void Update()
    {
        _currentDelay += Time.deltaTime;
    }

    public virtual void TakeDamage(AttackType attackedOf)
    {
        if (_takeDamageCooldown > _currentDelay)
            return;

        _health--;
        _currentDelay = 0;
        CheckAlife();
    }

    public abstract void Died();

    private void CheckAlife()
    {
        if (_health <= 0)
        {
            Dying?.Invoke(this);
            Died();
        }
    }
}
