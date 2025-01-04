using UnityEngine;

public class Boss_1 : Bosses
{
    [SerializeField] private Renderer _render;
    [SerializeField] private Color _shieldColor;

    private Color _baseColor;

    private void OnEnable()
    {
        if (_render != null)
            _baseColor = _render.material.color;
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
            _render.material.color = _shieldColor;
        }
        else
        {
            _render.material.color = _baseColor;
        }

        base.ChangeAction();
    }
}
