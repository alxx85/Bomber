using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    private const int BoostAmount = 1;
    private const int BoostZero = 0;
    private const float BoostSpeedRate = .5f;

    [Header("Level")]
    //[SerializeField] private GameLevels _levels;
    //[SerializeField] private WorldGenerator _world;
    [Header("Player")]
    [SerializeField] private PlayerMover _player;
    [SerializeField] private PlayerMover _playerTemplate;
    [SerializeField] private Bomb _templateBomb;
    [SerializeField] private int _currentLevel = 0;
    [SerializeField] private int _lifes;
    [SerializeField] private float _speed;
    [SerializeField] private int _bombAmount;
    [SerializeField] private int _bombPower;
    [SerializeField] private bool _canKickBomb;
    [SerializeField] private bool _useShield;
    [Header("Limit Properties")]
    [SerializeField] private int _maxSpeed = 6;
    [SerializeField] private int _maxBombAmount = 8; 
    [SerializeField] private int _maxBombPower = 10;

    [Header("Player Input")]
    public KeyCode LeftKey;
    public KeyCode RightKey;
    public KeyCode ForwardKey;
    public KeyCode BackKey;
    public KeyCode SetBombKey;
    public KeyCode KickBombKey;

    private List<Character> _levelEnemys = new List<Character>();
    private List<Bomb> _bombPool = new List<Bomb>();
    private List<LevelSetting> _level = new List<LevelSetting>();
    private Portal _portal;

    public int Lifes => _lifes;
    public float Speed => _speed;
    public int Bomb => _bombAmount;
    public int Power => _bombPower;
    public bool CanKick => _canKickBomb;
    public bool UseShield => _useShield;
    public int Width => _level[_currentLevel].Width;
    public int Height => _level[_currentLevel].Height;
    public List<Bomb> BombPool => _bombPool;
    public PlayerMover Player => _playerTemplate;

    public event Action ChangedPlayerProperties;

    private void Awake()
    {
        if (GameSettings.Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);

        LoadLevels();
    }

    private void OnDisable()
    {
        if (_player != null)
        {
            _player.PickUpBooster -= OnPickup;
            _player.GetComponent<Character>().Dying -= OnPlayerDying;
        }

        foreach (var enemy in _levelEnemys)
        {
            enemy.Dying -= OnEnemyDying;
        }
    }

    public LevelSetting GetCurrentLevel()
    {
        LevelSetting level = _level[_currentLevel];
        return level;
    }

    public void InitPlayer(PlayerMover player)
    {
        _player = player;
        _player.PickUpBooster += OnPickup;
        _player.GetComponent<Character>().Dying += OnPlayerDying;
    }

    public void InitLevelPortal(Portal portal)
    {
        _portal = portal;
        _portal.ChangedLevel += OnChangedLevel;
        CreatBombPool();
    }

    public void AddEnemyOnList(Character enemy)
    {
        _levelEnemys.Add(enemy);
        enemy.Dying += OnEnemyDying;
    }

    private void OnChangedLevel(Portal portal)
    {
        portal.ChangedLevel -= OnChangedLevel;
        _portal = null;
        _currentLevel++;
    }

    private void LoadLevels()
    {
        var levels = Resources.LoadAll("Levels/", typeof(LevelSetting));

        foreach (var item in levels)
        {
            _level.Add((LevelSetting)item);
        }
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
            _portal.Activate();
        }
    }

    private void OnPlayerDying(Character player)
    {
        _lifes--;
    }

    private void OnPickup(Boost booster)
    {
        _lifes += booster.Life ? BoostAmount : BoostZero;
        
        if (_speed < _maxSpeed)
            _speed += booster.Speed ? BoostAmount * BoostSpeedRate : BoostZero;

        if (_bombAmount < _maxBombAmount)
            _bombAmount += booster.BombAmount ? BoostAmount : BoostZero;
        
        if (_bombPower < _maxBombPower)
            _bombPower += booster.BombPower ? BoostAmount : BoostZero;

        if (_canKickBomb == false)
            _canKickBomb = booster.Kick;

        if (_useShield == false)
            _useShield = booster.Shield;

        Debug.Log("Pickup");
        ChangedPlayerProperties?.Invoke();
    }
}