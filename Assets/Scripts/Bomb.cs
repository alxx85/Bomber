using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private const int BlockCount = 2;

    [SerializeField] private Fire _firePrefab;
    [SerializeField] private LayerMask _destroyedMask;
    [SerializeField] private LayerMask _charactersMask;
    [SerializeField] private AnimationCurve _sizeAnimation;
    [SerializeField] private AnimationCurve _colorAnimation;
    [SerializeField] private Renderer[] _bombMaterial;

    private GameSettings _setting;
    private PlayerAttacks _player;
    private Collider _collider;
    private List<Destroying> _destroying = new List<Destroying>();
    private List<Character> _damaging= new List<Character>();
    private Coroutine _startDelay;
    private Coroutine _collision;
    private Rigidbody _body;
    private int _timer;
    private int _power;
    private bool _isActivated = false;
    private bool _canStartAttack = true;
    private float _currentTime = 0;
    private float _currentColorTime = 0;
    private float _totalTime;

    private void Start()
    {
        _totalTime = _sizeAnimation.keys[_sizeAnimation.keys.Length - 1].time;
        _setting = GameSettings.Instance;
        _collider = GetComponent<Collider>();
        _body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float currentSize = _sizeAnimation.Evaluate(_currentTime);
        transform.localScale = new Vector3(transform.localScale.x, currentSize, transform.localScale.z);
        _currentTime += Time.deltaTime;

        if (_currentTime >= _totalTime)
            _currentTime -= _totalTime;
        
        _currentColorTime += Time.deltaTime;

        for (int i = 0; i < _bombMaterial.Length; i++)
            _bombMaterial[i].material.color = new Color(_colorAnimation.Evaluate(_currentColorTime), 0, 0);
    }

    public void Init(int TimeToActivate, int power, PlayerAttacks character)
    {
        if (_isActivated == false)
        {
            _timer = TimeToActivate;
            _currentColorTime = -TimeToActivate + 1;
            _power = power;

            if (character != null)
                _player = character;

            _startDelay = StartCoroutine(DelayingAttack());
            _collision = StartCoroutine(GetCollision());
        }
        else if (_canStartAttack)
        {
            _timer = 0;
            StopCoroutine(_startDelay);
            StartAttack();
        }
    }

    public void Move(Vector3 direction)
    {
        _body.velocity = direction * 5;
    }

    private IEnumerator GetCollision()
    {
        Collider[] hit;
        do
        {
            yield return new WaitForSeconds(0.2f);
            hit = Physics.OverlapSphere(transform.position, 0.5f, _charactersMask);
        } while (hit.Length > 0);

        _collider.isTrigger = false;
    }

    private IEnumerator DelayingAttack()
    {
        _isActivated = true;

        while (_timer >= 1)
        {
            yield return new WaitForSeconds(1);
            _timer -= 1;
        }
        StartAttack();
    }

    private void StartAttack()
    {
        _canStartAttack = false;
        Activate();
        EndAttack();
    }

    private void EndAttack()
    {
        foreach (var item in _destroying)
        {
            item.Destroy();
        }

        foreach (var character in _damaging)
        {
            character.TakeDamage();
        }

        ResetState();
        Debug.Log(gameObject.name + " remove");
        _player.BombDeactivated(this);
    }

    private void Activate()
    {
        bool left = true, up = true, right = true, down = true;
        Vector3Int startPosition = ConvertPosition(transform.position);

        SetFireBlock(startPosition);

        for (int i = 1; i <= _power; i++)
        {
            if (left)
                left = SetFireBlock(startPosition, -i, 0);

            if (right)
                right = SetFireBlock(startPosition, i, 0);

            if (up)
                up = SetFireBlock(startPosition, 0, i);

            if (down)
                down = SetFireBlock(startPosition, 0, -i);
        }
    }

    private bool SetFireBlock(Vector3Int position, int x = 0, int z = 0)
    {
        if (position.x % 2 != 0)
            z = 0;

        if (position.z % 2 != 0)
            x = 0;

        Vector3 setPosition = new Vector3(position.x + x, 0, position.z + z);

        if (setPosition.x >= 0 && setPosition.x < _setting.Width - BlockCount && setPosition.z >= 0 
            && setPosition.z < _setting.Height - BlockCount)
        {
            Instantiate(_firePrefab, setPosition, Quaternion.identity);
            Collider[] hit = Physics.OverlapSphere(setPosition, 0.45f, _destroyedMask);

            foreach (var item in hit)
            {
                if (item.TryGetComponent(out Destroying destroy))
                {
                    _destroying.Add(destroy);
                    Debug.Log(destroy);
                    return false;
                }
                else if (item.TryGetComponent(out Bomb nextBomb))
                {
                    if (nextBomb != this)
                    {
                        Debug.Log(item.gameObject.name + " true " + Time.time);
                        nextBomb.Init(0, _power, null);
                        return false;
                    }
                }
                else if (item.TryGetComponent(out Character character))
                {
                    if (!_damaging.Contains(character))
                    {
                        _damaging.Add(character);
                        Debug.Log(character.name);
                    }
                    return true;
                }
            }
        }
        else
        {
            return false;
        }
        
        return true;
    }

    private Vector3Int ConvertPosition(Vector3 position) => new Vector3Int(Mathf.RoundToInt(position.x), 0, Mathf.RoundToInt(position.z));

    private void OnGUI()
    {
        Vector3 position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
        GUI.Label(new Rect(position.x, Screen.height - position.y, 50, 50),_timer.ToString());
    }

    private void ResetState()
    {
        StopCoroutine(_startDelay);
        StopCoroutine(_collision);
        _isActivated = false;
        _canStartAttack = true;
        _destroying.Clear();
        _damaging.Clear();
        _collider.isTrigger = true;
        _body.velocity = Vector3.zero;
    }
}