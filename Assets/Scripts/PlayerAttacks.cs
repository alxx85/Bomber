using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    private const string Jump = "Jump";
    private const string Force = "Force";

    [SerializeField] private int _bombDelay;
    [SerializeField] private LayerMask _destroyedMask;

    private List<Bomb> _bombsPool = new List<Bomb>();
    private List<Bomb> _installed = new List<Bomb>();
    private GameSettings _setting;
    private PlayerMover _mover;
    private int _bombAmound;
    private int _bombPower;
    private bool _canKick = false;
    private KeyCode _setBombButton = KeyCode.Space;
    private KeyCode _kickBombButton = KeyCode.E;

    private void Awake()
    {
        _mover = GetComponent<PlayerMover>();
    }

    private void OnDisable()
    {
        _setting.ChangedPlayerProperties -= InitAttack;
    }

    private void Start()
    {
        _setting = GameSettings.Instance;
        _setting.ChangedPlayerProperties += InitAttack;
        InitAttack();
    }

    private void Update()
    {
        if(Input.GetKeyDown(_setBombButton))
        {
            if (_bombsPool.Count > 0 && _bombAmound > 0)
            {
                BombInstall();
                _bombAmound--;
            }
        }

        if (Input.GetKeyDown(_kickBombButton) && _canKick)
        {
            Vector3 direction = _mover.Direction;
            Vector3 bombPosition = GetRoundPosition() + direction;
            Debug.Log("Force");
            Collider[] hit = Physics.OverlapSphere(bombPosition, 0.45f, _destroyedMask);
            
            if (hit.Length > 0)
            {

                if (hit[0].TryGetComponent(out Bomb bomb))
                {
                    bomb.Move(direction);
                    Debug.Log($"Move bomb to {direction}");
                }
            }
        }

    }

    public void BombDeactivated(Bomb bomb)
    {
        _installed.Remove(bomb);
        _bombsPool.Add(bomb);
        _bombAmound++;
        bomb.gameObject.SetActive(false);
    }

    private void InitAttack()
    {
        _bombAmound = _setting.Bomb;
        _bombPower = _setting.Power;
        _canKick = _setting.CanKick;
        _bombsPool = _setting.BombPool;
        _setBombButton = _setting.SetBombKey;
        _kickBombButton = _setting.KickBombKey;
    }

    private void BombInstall()
    {
        Bomb bomb = _bombsPool[0];
        _installed.Add(bomb);
        _bombsPool.Remove(bomb);
        bomb.transform.position = GetRoundPosition();
        bomb.gameObject.SetActive(true);
        bomb.Init(_bombDelay, _bombPower, this);
    }

    private Vector3 GetRoundPosition()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.RoundToInt(currentPosition.x);
        currentPosition.y = 0f;
        currentPosition.z = Mathf.RoundToInt(currentPosition.z);
        return currentPosition;
    }

}
