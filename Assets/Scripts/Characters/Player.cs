public class Player : Characters
{
    private GameSettings _setting;
    private int _startedHealth;

    private void Start()
    {
        _setting = GameSettings.Instance;
        _startedHealth = _health;
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
        _health = _startedHealth;
    }

}
