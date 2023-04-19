using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _activateParticle;
    [SerializeField] private float _timeToSpawn;

    private GameSettings _settings;
    [SerializeField] private Character _player;
    private bool _isActiv = false;
    private bool _canSpawn;
    private float _delay;

    public event Action<Portal> ChangedLevel;

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

                _canSpawn = false;
            }
        }
    }

    private void OnDisable()
    {
        if (_player != null)
            _player.GetComponent<Character>().Dying -= OnPlayerDying;
    }

    public void Init()//PlayerMover player)
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
        if (_player == null)
        {
            var player = Instantiate(_settings.Player.gameObject, transform.position, Quaternion.identity);
            _player = player.GetComponent<Character>();
            _player.Dying += OnPlayerDying;
        }
        else
        {
            _player.transform.position = transform.position;
            _player.gameObject.SetActive(true);
        }
    }

    private void OnPlayerDying(Character player)
    {
        _delay = _timeToSpawn;
        _canSpawn = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isActiv)
        {
            if (other.TryGetComponent(out PlayerMover player))
            {
                Debug.Log("Level Completed!");
                ChangedLevel?.Invoke(this);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
