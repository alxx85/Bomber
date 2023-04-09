using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    private const string Jump = "Jump";
    private const string Force = "Force";

    [SerializeField] private Bomb _templateBomb;
    [SerializeField] private int _bombDelay;
    [SerializeField] private int _bombCount;
    [SerializeField] private int _power;
    [SerializeField] private LayerMask _destroyedMask;

    private List<Bomb> _bombsPool = new List<Bomb>();
    private List<Bomb> _installed = new List<Bomb>();
    private PlayerMover _mover;

    private void Awake()
    {
        for (int i = 0; i < _bombCount; i++)
        {
            Bomb bomb = Instantiate(_templateBomb, Vector3.zero, Quaternion.identity);
            bomb.gameObject.SetActive(false);
            _bombsPool.Add(bomb);
        }

        _mover = GetComponent<PlayerMover>();
    }

    private void Update()
    {
        if(Input.GetButtonDown(Jump))
        {
            if (_bombsPool.Count > 0)
                BombInstall();
        }

        if (Input.GetButtonDown(Force))
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
        bomb.gameObject.SetActive(false);
    }

    private void BombInstall()
    {
        Bomb bomb = _bombsPool[0];
        _installed.Add(bomb);
        _bombsPool.Remove(bomb);
        bomb.transform.position = GetRoundPosition();
        bomb.gameObject.SetActive(true);
        bomb.Init(_bombDelay, _power, this);
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
