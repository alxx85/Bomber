using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    [Header("Level")]
    [SerializeField] private int _levelNumber;
    [SerializeField] private WorldGenerator _world;
    [Header("Player")]
    [SerializeField] private PlayerMover _player;
    [SerializeField] private Bomb _templateBomb;
    [SerializeField] private int _currentLevel;
    [SerializeField] private int _lifes;
    [SerializeField] private int _speed;
    [SerializeField] private int _bombAmount;
    [SerializeField] private int _bombPower;
    [SerializeField] private bool _canKickBomb;
    [SerializeField] private bool _useShield;
    [SerializeField] private int _maxBombAmount = 8; 

    private List<Character> _levelEnemys = new List<Character>();
    private List<Bomb> _bombPool = new List<Bomb>();
    private List<LevelSetting> _levels = new List<LevelSetting>();

    public int Lifes => _lifes;
    public int Speed => _speed;
    public int Bomb => _bombAmount;
    public int Power => _bombPower;
    public bool CanKick => _canKickBomb;
    public bool UseShield => _useShield;
    public int Width => _levels[_currentLevel - 1].Width;
    public int Height => _levels[_currentLevel - 1].Height;
    public List<Bomb> BombPool => _bombPool;


    public event Action ChangedPlayerProperties;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this)
            Destroy(gameObject);

        DontDestroyOnLoad(this);

        _levels.Add(new LevelSetting(1, 15, 15, 1, 25, 2));

        CreatBombPool();
        _world.InitWorldSetting(this, _levels[_currentLevel - 1]);
    }

    private void OnEnable()
    {
        _player.PickUpBooster += OnPickup;
    }

    private void OnDisable()
    {
        _player.PickUpBooster -= OnPickup;

        foreach (var enemy in _levelEnemys)
        {
            enemy.Dying -= OnEnemyDying;
        }
    }

    public void AddEnemyOnList(Character enemy)
    {
        _levelEnemys.Add(enemy);
        enemy.Dying += OnEnemyDying;
    }

    private void CreatBombPool()
    {
        for (int i = 0; i < _maxBombAmount; i++)
        {
            Bomb bomb = Instantiate(_templateBomb, Vector3.zero, Quaternion.identity);
            bomb.gameObject.SetActive(false);
            _bombPool.Add(bomb);
        }
    }

    private void OnEnemyDying(Character enemy)
    {
        enemy.Dying -= OnEnemyDying;
        _levelEnemys.Remove(enemy);
        // Add Bonus

        if (_levelEnemys.Count == 0)
        {
            Debug.Log("Player Win");
        }
    }

    private void OnPickup(Boost booster)
    {
        _lifes += booster.Life ? 1 : 0;
        _speed += booster.Speed ? 1 : 0;

        if (_bombAmount < _maxBombAmount)
            _bombAmount += booster.BombAmount ? 1 : 0;
        
        _bombPower += booster.BombPower ? 1 : 0;

        if (_canKickBomb == false)
            _canKickBomb = booster.Kick;

        if (_useShield == false)
            _useShield = booster.Shield;

        Debug.Log("Pickup");
        ChangedPlayerProperties?.Invoke();
    }
}


public class LevelSetting
{
    public int Level { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int BoostAmount { get; private set; }
    public int BrickBlockAmount { get; private set; }
    public int EnemyAmount { get; private set; }

    public LevelSetting(int level, int width, int height, int boostAmount, int brickAmount, int enemyAmount)
    {
        Level = level;
        Width = width;
        Height = height;
        BoostAmount = boostAmount;
        BrickBlockAmount = brickAmount;
        EnemyAmount = enemyAmount;
    }
}

public class Boost
{
    public bool Life { get; private set; }
    public bool Speed { get; private set; }
    public bool BombAmount { get; private set; }
    public bool BombPower { get; private set; }
    public bool Kick { get; private set; }
    public bool Shield { get; private set; }

    public Boost(bool life, bool speed, bool amount, bool power, bool kick, bool shield)
    {
        Life = life;
        Speed = speed;
        BombAmount = amount;
        BombPower = power;
        Kick = kick;
        Shield = shield;
    }
}
