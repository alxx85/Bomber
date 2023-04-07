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
    private bool _stopAtBlock = true;
    private bool _isActivated = false;
    private bool _canStartAttack = true;
    //private PlayerMovement _player;
    private Collider _collider;
    private List<Destroying> _destroying = new List<Destroying>();
    private Coroutine _activate;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void Init(int TimeToActivate, int power)//, PlayerMovement character)
    {
        //StartCoroutine(GetCollision());

        if (_isActivated == false)
        {
            _timer = TimeToActivate;
            _power = power;

            //if (character != null)
            //    _player = character;
          
            _activate = StartCoroutine(DelayingAttack());
        }
        else if (_canStartAttack)
        {
            _timer = 0;
            StopCoroutine(_activate);
            StartAttack();
        }
    }

    private IEnumerator GetCollision()
    {
        Collider[] hit;
        do
        {
            yield return new WaitForSeconds(0.1f);
            hit = Physics.OverlapSphere(transform.position, 0.4f, _charactersMask);
            //Debug.Log(hit[0].gameObject.name);
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
        Invoke(nameof(EndAttack), .5f);
    }

    private void EndAttack()
    {
        foreach (var item in _destroying)
        {
            item.Destroy();
        }

        //MoveController.SetBombs.Remove(gameObject);
        //_player.BombDeactivated(this);
        Debug.Log(gameObject.name + " remove");
        Destroy(gameObject);
    }

    private void Activate()
    {
        bool left = true, up = true, right = true, down = true;
        Vector3Int startPosition = ConvertPosition(transform.position);

        SetFireBlock(startPosition);

        for (int i = 1; i <= _power; i++)
        {
            if (left || !_stopAtBlock)
                left = SetFireBlock(startPosition, -i, 0);

            if (right || !_stopAtBlock)
                right = SetFireBlock(startPosition, i, 0);

            if (up || !_stopAtBlock)
                up = SetFireBlock(startPosition, 0, i);

            if (down || !_stopAtBlock)
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
            Instantiate(_firePrefab, setPosition, Quaternion.identity, transform);
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
                        nextBomb.Init(0, _power);//, null);
                        return false;
                    }
                }
            }
        }
        else
        {
            return false;
        }
        
        return true;
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.TryGetComponent(out PlayerMovement player))
    //        _collider.isTrigger = false;
    //}

    private Vector3Int ConvertPosition(Vector3 position) => new Vector3Int((int) position.x, 0, (int) position.z);

    private void OnGUI()
    {
        Vector3 position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
        GUI.Label(new Rect(position.x, Screen.height - position.y, 100, 100),_timer.ToString());
    }
}
