public class Player : Characters
{
    private GameSettings _setting;

    private void Start()
    {
        _setting = GameSettings.Instance;
    }

    public override void TakeDamage(AttackType attackedOf)
    {
        if (attackedOf == AttackType.Bomb)
        {
            if (_setting.UseShield)
                return;
        }

        base.TakeDamage(attackedOf);
    }

    public override void Died()
    {
        gameObject.SetActive(false);
    }
}
