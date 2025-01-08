using System;
using UnityEngine;

public class Boss_1 : Bosses
{
    [SerializeField] private int _stopActionDelay = 10;
    [SerializeField] private Renderer _render;
    [SerializeField] private Color _damageColor;

    private Color _baseColor;

    public override event Action<int, int> ChangedTimer;

    private void OnEnable()
    {
        if (_render != null)
            _baseColor = _render.material.color;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isActiveAction)
        {
            if (actionTimer >= _stopActionDelay)
            {
                isActiveAction = false;
                ChangeAction();
            }
        }
    }

    public override void TakeDamage(AttackType attackedOf)
    {
        if (isActiveAction == false)
            return;

        base.TakeDamage(attackedOf);
    }

    protected override void ChangeAction()
    {
        if (isActiveAction)
        {
            _render.material.color = _damageColor;
        }
        else
        {
            _render.material.color = _baseColor;
        }

        base.ChangeAction();
    }

    protected override void TimerTick()
    {
        if (isActiveAction)
            ChangedTimer?.Invoke(actionTimer, _stopActionDelay);
        else
            ChangedTimer?.Invoke(actionTimer, startActionDelay);
    }
}
