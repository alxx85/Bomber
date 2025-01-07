using UnityEngine;

public abstract class Bosses : Characters
{
    [SerializeField] protected float _startActionDelay = 10f;

    protected float _actionTimer = 0;
    protected bool _isActiveAction = false;

    protected virtual void FixedUpdate()
    {
        _actionTimer += Time.fixedDeltaTime;

        if (_isActiveAction == false)
        {
            if (_actionTimer >= _startActionDelay)
            {
                _isActiveAction = true;
                ChangeAction();
            }
        }
    }

    public override void Died()
    {
        Destroy(gameObject);
    }

    protected virtual void ChangeAction()
    {
        _actionTimer = 0;
    }
}
