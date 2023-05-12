using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private const float MaxSpawnedDelay = -25;
    private const float MinEnemyDistance = 2.5f;

    [SerializeField] private ParticleSystem[] _activateParticle;
    [SerializeField] private float _timeToSpawn;
    [SerializeField] private LayerMask _enemyMask;

    private GameSettings _settings;
    private Characters _player;
    private bool _isActiv = false;
    private bool _canSpawn;
    private bool _canEndLevel = false;
    private bool _playerSpawned = false;
    private float _delay;

    public event Action<Portal, bool> ChangedLevel;

    private void Start()
    {
        _settings = GameSettings.Instance;
        _settings.InitLevelPortal(this);
        _delay = _timeToSpawn;
        _canSpawn = true;
    }

    private void Update()
    {
        if (_canSpawn)
        {
            _delay -= Time.deltaTime;

            if (_delay <= 0)
            {
                if (_settings.Lifes > 0)
                    SpawnPlayerWithDelay();

                if (_playerSpawned)
                {
                    _canSpawn = false;
                    _delay = 0;
                }

                if (_delay < MaxSpawnedDelay)
                {
                    ChangedLevel?.Invoke(this, false);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
    }

    private void OnDisable()
    {
        if (_player != null)
            _player.GetComponent<Characters>().Dying -= OnPlayerDying;
    }

    public void Init()
    {
        _delay = _timeToSpawn;
        _canSpawn = true;
    }

    public void Activate()
    {
        _isActiv = true;

        foreach (var particle in _activateParticle)
            particle.Play();
    }

    private void SpawnPlayerWithDelay()
    {
        Collider[] enemy = Physics.OverlapSphere(transform.position, MinEnemyDistance, _enemyMask);
        
        if (enemy.Length == 0)
        {
            if (_player == null)
            {
                var player = Instantiate(_settings.Player.gameObject, transform.position, Quaternion.identity);
                _player = player.GetComponent<Characters>();
                _player.Dying += OnPlayerDying;
                _settings.InitPlayer(_player);
            }
            else
            {
                if (_settings.Lifes >= 0)
                {
                    _player.transform.position = transform.position;
                    _player.gameObject.SetActive(true);
                }
            }
            _playerSpawned = true;
        }
    }

    private void OnPlayerDying(Characters player)
    {
        _delay = _timeToSpawn;
        _canSpawn = true;
        _playerSpawned = false;
        _canEndLevel = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isActiv)
        {
            if (_canEndLevel)
            {
                if (other.TryGetComponent(out PlayerMovement player))
                {
                    Debug.Log("Level Completed!");
                    ChangedLevel?.Invoke(this, true);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
            _canEndLevel = true;
    }
}
