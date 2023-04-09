using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private Fire _firePrefab;
    [SerializeField] private LayerMask _destroyedMask;
    [SerializeField] private LayerMask _charactersMask;
    
    private int _timer;
    private int _power;
    private bool _isActivated = false;
    private bool _canStartAttack = true;
    private PlayerAttacks _player;
    private Collider _collider;
    private List<Destroying> _destroying = new List<Destroying>();
    private List<Character> _damaging= new List<Character>();
    private Coroutine _startDelay;
    private Coroutine _collision;
    private Rigidbody _body;


    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _body = GetComponent<Rigidbody>();
    }

    public void Init(int TimeToActivate, int power, PlayerAttacks character)
    {
        if (_isActivated == false)
        {
            _timer = TimeToActivate;
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

        if (setPosition.x >= 0 && setPosition.x < 23 && setPosition.z >= 0 && setPosition.z < 13)
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
        GUI.Label(new Rect(position.x, Screen.height - position.y, 100, 100),_timer.ToString());
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