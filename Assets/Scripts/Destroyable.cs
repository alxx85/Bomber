using UnityEngine;

public class Destroyable : MonoBehaviour, IDamageable
{
    public void TakeDamage()
    {
        Destroy(gameObject);
    }
}
