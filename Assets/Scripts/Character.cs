using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private bool _isPlayer;

    private GameSettings _setting;
    private bool _useShield;

    public event Action<Character> Dying;

    private void OnDisable()
    {
        if (_isPlayer)
            _setting.ChangedPlayerProperties -= InitPlayer;
    }

    private void Start()
    {
        _setting = GameSettings.Instance;

        if (_isPlayer)
        {
            _setting.ChangedPlayerProperties += InitPlayer;
            InitPlayer();
        }
    }

    public void TakeDamage()
    {
        if (_useShield)
            _useShield = false;
        else
            _health--;

        if (_health <= 0)
        {
            Destroy(gameObject);
            Dying?.Invoke(this);
        }
    }

    private void InitPlayer()
    {
        _useShield = _setting.UseShield;
    }
}
