using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private bool _isPlayer;
    [SerializeField] private bool _isBoss;

    private EnemyBossMover _bossMover;
    private GameSettings _setting;
    private bool _useShield;

    public event Action<Character> Dying;

    private void OnDisable()
    {
        if (_isPlayer && _setting != null)
            _setting.ChangedPlayerProperties -= InitPlayer;

        if (_isBoss)
        {
            _bossMover.UsedShield += OnUsedShield;
        }
    }

    private void Start()
    {
        _setting = GameSettings.Instance;

        if (_isPlayer)
        {
            _setting.ChangedPlayerProperties += InitPlayer;
            InitPlayer();
        }
        else if (_isBoss)
        {
            _bossMover = GetComponent<EnemyBossMover>();
            _bossMover.UsedShield += OnUsedShield;
        }
    }

    public void TakeDamage()
    {
        if (_useShield)
        {
            if (_isPlayer)
                _useShield = false;
        }
        else
        {
            _health--;
        }

        if (_health <= 0)
        {
            if (_isPlayer == false)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);

            Dying?.Invoke(this);
        }
    }

    private void OnUsedShield(bool activate)
    {
        _useShield = activate;
    }

    private void InitPlayer()
    {
        _useShield = _setting.UseShield;
    }
}
