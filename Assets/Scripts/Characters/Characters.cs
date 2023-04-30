using System;
using UnityEngine;

public abstract class Characters : MonoBehaviour, IDamageable
{
    [SerializeField] protected int _health;
    //[SerializeField] private bool _isPlayer;
    //[SerializeField] private bool _isBoss;
    [SerializeField] private float _takeDamageCooldown;

    //private GameSettings _setting;
    //private bool _useShield;
    private float _currentDelay;

    public event Action<Characters> Dying;

    //private void Start()
    //{
    //    if (_isPlayer)
    //        _setting = GameSettings.Instance;
    //}

    private void Update()
    {
        _currentDelay += Time.deltaTime;
    }

    public virtual void TakeDamage(AttackType attackedOf)
    {
        //if (_useShield == false || attackedOf == AttackType.Enemy)
        //{
        if (_takeDamageCooldown > _currentDelay)
            return;

        _health--;
        _currentDelay = 0;
        CheckAlife();

        //}

    }

    public abstract void Died();
    //{
    //    Destroy(gameObject);
    //}

    private void CheckAlife()
    {
        if (_health <= 0)
        {
            Dying?.Invoke(this);
            Died();
        }
    }

    //private void OnUsedShield(bool activate)
    //{
    //    _useShield = activate;
    //}
}
