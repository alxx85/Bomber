using UnityEngine;

public class Boss_1 : Bosses
{
    [SerializeField] private float _stopActionDelay = 10f;
    [SerializeField] private Renderer _render;
    [SerializeField] private Color _damageColor;

    private Color _baseColor;

    private void OnEnable()
    {
        if (_render != null)
            _baseColor = _render.material.color;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_isActiveAction)
        {
            if (_actionTimer >= _stopActionDelay)
            {
                _isActiveAction = false;
                ChangeAction();
            }
        }

    }

    public override void TakeDamage(AttackType attackedOf)
    {
        if (_isActiveAction)
            return;

        base.TakeDamage(attackedOf);
    }

    protected override void ChangeAction()
    {
        if (_isActiveAction)
        {
            _render.material.color = _damageColor;
        }
        else
        {
            _render.material.color = _baseColor;
        }

        base.ChangeAction();
    }
}
