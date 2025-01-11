using UnityEngine;

public class BoostViewer : MonoBehaviour, IDamageable
{
    [SerializeField] private Booster _booster;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private AudioSource _pickupSFX;

    private bool _isActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
        {
            Destroy(gameObject, 0.2f);
            GameSettings.Instance.PickupBooster(_booster.GetBoost);
            _pickupSFX.Play();
            Destroy(this);
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
