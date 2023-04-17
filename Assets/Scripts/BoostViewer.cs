using UnityEngine;

public class BoostViewer : MonoBehaviour
{
    [SerializeField] private Booster _booster;
    [SerializeField] private SpriteRenderer _renderer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMover player))
        {
            Destroy(gameObject);
            player.PickUp(_booster.AddBoost);//(new Boost(_life, _speed, _addBombAmount, _addBombPower, _kick, _shield));
        }
    }
    
    public void Init(Booster boost)
    {
        _booster = boost;
        _renderer.sprite = _booster.Sprite;
        _renderer.color = _booster.BackgroundColor;
    }
}
