using UnityEngine;

public abstract class Bosses : Characters
{
    [SerializeField] private float _startActionDelay = 10f;
    [SerializeField] private float _stopActionDelay = 10f;
    [SerializeField] private Renderer _render;
    [SerializeField] private Color _shieldColor;

    protected float _actionTimer = 0;
    private bool _isActiveAction = false;
    private Color _baseColor;

    private void Start()
    {
        if (_render != null)
            _baseColor = _render.material.color;
    }

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

    public override void TakeDamage(AttackType attackedOf)
    {
        if (_isActiveAction == false)
            base.TakeDamage(attackedOf);
    }

    protected virtual void ChangeAction()
    {
        if (_isActiveAction)
        {
            _render.material.color = _shieldColor;
        }
        else
        {
            _render.material.color = _baseColor;
        }

        _actionTimer = 0;
    }
}
