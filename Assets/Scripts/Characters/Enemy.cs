public class Enemy : Characters
{
    public override void Died()
    {
        Destroy(gameObject);
    }
}
