using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    private const int BoostAmount = 1;
    private const int BoostZero = 0;
    private const float BoostSpeedRate = .5f;

    [Header("Player")]
    [SerializeField] private PlayerMovement _playerTemplate;
    [SerializeField] private Bomb _templateBomb;
    [SerializeField] private int _currentLevel = 0;
    [SerializeField] private int _lifes;
    [SerializeField] private float _speed;
    [SerializeField] private int _bombAmount;
    [SerializeField] private int _bombPower;
    [SerializeField] private bool _canKickBomb;
    [SerializeField] private bool _useShield;
    [Header("Bomb Properties")]
    [SerializeField] private float _explodeDelay = 3f;
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

    private List<Characters> _levelEnemys = new List<Characters>();
    private List<LevelSetting> _levels = new List<LevelSetting>();
    private BossStatsViewer _bossStatsViewer;
    private Characters _player;
    private Portal _portal;
    private float _startSpeed;
    private int _currentTime;
    private WaitForSeconds _timer = new WaitForSeconds(1f);
    private Coroutine _coroutineTimer;

    public int Lifes => _lifes;
    public float Speed => _speed;
    public float SpeedLevel => (_speed - _startSpeed) / BoostSpeedRate;
    public int Bomb => _bombAmount;
    public int Power => _bombPower;
    public bool CanKick => _canKickBomb;
    public bool UseShield => _useShield;
    public int Width => _levels[_currentLevel].Width;
    public int Height => _levels[_currentLevel].Height;
    public float ActivateDelay => _explodeDelay;
    public PlayerMovement Player => _playerTemplate;
    public bool LevelClear => _levelEnemys.Count == 0;
    public BossStatsViewer BossStatsViewer => _bossStatsViewer;

    public event Action<int, int> ChangedLevelTime;
    public event Action<int> ChangedEnemyCount;
    public event Action LevelTimeEnded;
    public event Action ChangedPlayerProperties;

    private void Awake()
    {
        if (GameSettings.Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);
        _startSpeed = _speed;
        LoadLevels();
    }

    private void OnDisable()
    {
        if (_player != null)
        {
            _player.Dying -= OnPlayerDying;
        }

        foreach (var enemy in _levelEnemys)
        {
            enemy.Dying -= OnEnemyDying;
        }
    }

    public void InitBossStats(BossStatsViewer viewer)
    {
        _bossStatsViewer = viewer;
    }

    public int GetLevelNumber() => _currentLevel;

    public void StartLevelTimer(int delay)
    {
        _currentTime = delay;
        ChangedLevelTime?.Invoke(_currentTime, delay);
        //_coroutineTimer = StartCoroutine(TimeTick(delay));
    }

    public LevelSetting GetCurrentLevel()
    {
        int index = _levels.FindIndex(level => level.name == _currentLevel.ToString());
        LevelSetting level = _levels[index];
        return level;
    }

    public void InitPlayer(Characters player)
    {
        _player = player;
        _player.Dying += OnPlayerDying;
    }

    public void InitLevelPortal(Portal portal)
    {
        _portal = portal;
        _portal.ChangedLevel += OnChangedLevel;
        _coroutineTimer = StartCoroutine(TimeTick(_currentTime));
    }

    public void AddEnemyOnList(Characters enemy)
    {
        _levelEnemys.Add(enemy);
        enemy.Dying += OnEnemyDying;
        ChangedEnemyCount?.Invoke(_levelEnemys.Count);
    }

    public void PickupBooster(Boost boost)
    {
        ChangePlayerProperties(boost);
    }

    private void OnChangedLevel(Portal portal, bool nextLevel)
    {
        portal.ChangedLevel -= OnChangedLevel;
        _portal = null;
        StopCoroutine(_coroutineTimer);
        _coroutineTimer = null;
        _levelEnemys.Clear();

        if (nextLevel && _currentLevel < _levels.Count - 1)
            _currentLevel++;
    }

    private void LoadLevels()
    {
        var levels = Resources.LoadAll("Levels/", typeof(LevelSetting));
        
        foreach (var item in levels)
        {
            _levels.Add((LevelSetting)item);
        }
    }

    private void OnEnemyDying(Characters enemy)
    {
        enemy.Dying -= OnEnemyDying;
        _levelEnemys.Remove(enemy);
        ChangedEnemyCount?.Invoke(_levelEnemys.Count);

        if (_levelEnemys.Count == 0)
        {
            _portal.Activate();
        }
    }

    private void OnPlayerDying(Characters player)
    {
        _lifes--;
        ChangedPlayerProperties?.Invoke();
        CheckAlive();
    }

    private void CheckAlive()
    {
        if (_lifes <= 0)
        {
            //Show player dying screen;
            Debug.Log("Player is die!\nYou lose game! :)");
        }
    }

    private void ChangePlayerProperties(Boost booster)
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

        ChangedPlayerProperties?.Invoke();
    }

    private IEnumerator TimeTick(int timer)
    {
        ChangedEnemyCount?.Invoke(_levelEnemys.Count);

        do
        {
            yield return _timer;
            _currentTime--;
            _currentTime = Mathf.Clamp(_currentTime, 0, timer);
            ChangedLevelTime?.Invoke(_currentTime, timer);
        } while (_currentTime > 0);
        LevelTimeEnded?.Invoke();
    }
}