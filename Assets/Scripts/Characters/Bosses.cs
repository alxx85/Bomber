using UnityEngine;

public abstract class Bosses : Characters
{
    [SerializeField] private float _startActionDelay = 10f;
    [SerializeField] private float _stopActionDelay = 10f;

    protected float _actionTimer = 0;
    protected bool _isActiveAction = false;

    private void FixedUpdate()
    {
        _actionTimer += Time.fixedDeltaTime;

        if (_isActiveAction == false)
        {
            if (_actionTimer >= _startActionDelay)
            {
                ChangeAction();
                _isActiveAction = true;
            }
        }
        else
        {
            if (_actionTimer >= _stopActionDelay)
            {
                ChangeAction();
                _isActiveAction = false;
            }
        }
    }

    public override void Died()
    {
        Destroy(gameObject);
    }

    //public override void TakeDamage(AttackType attackedOf)
    //{
    //    if (_isActiveAction == false)
    //        base.TakeDamage(attackedOf);
    //}

    protected virtual void ChangeAction()
    {
        _actionTimer = 0;
    }
}
