using UnityEngine;

public class BoostViewer : MonoBehaviour
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

    public void TakeDamage()
    {
        if (_isActive == true)
            Destroy(gameObject);
        else
            _isActive = true;
    }
}
