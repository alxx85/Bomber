using UnityEngine;

public class BoostViewer : MonoBehaviour, IDamageable
{
    [SerializeField] private Booster _booster;
    [SerializeField] private SpriteRenderer _renderer;

    private bool _isActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
        {
            Destroy(gameObject);
            GameSettings.Instance.PickupBooster(_booster.GetBoost);
        }
    }
    
    public void Init(Booster boost)
    {
        _booster = boost;
        _renderer.sprite = _booster.Sprite;
        _renderer.color = _booster.BackgroundColor;
    }

    public void TakeDamage(AttackType attackedOf)
    {
        if (_isActive == true && attackedOf == AttackType.Bomb)
            Destroy(gameObject);
        else
            _isActive = true;
    }
}
