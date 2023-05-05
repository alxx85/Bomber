//using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    //[SerializeField] private int _bombDelay;
    [SerializeField] private Bomb _template;
    [SerializeField] private LayerMask _destroyedMask;

    private List<Bomb> _bombsInstalled = new List<Bomb>();
    private GameSettings _setting;
    //private PlayerMovement _mover;
    private PlayerInput _input;

    private void Awake()
    {
        //_mover = GetComponent<PlayerMovement>();
        _input = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _input.SetedBomb += OnSetedBomb;
        _input.KickedBomb += OnKickedBomb;
    }

    private void OnDisable()
    {
        if (_bombsInstalled.Count > 0)
        {
            foreach (var bomb in _bombsInstalled)
            {
                bomb.Exploded -= OnExploded;
            }
            _bombsInstalled.Clear();
        }

        _input.SetedBomb -= OnSetedBomb;
        _input.KickedBomb -= OnKickedBomb;
    }

    private void Start()
    {
        _setting = GameSettings.Instance;
    }

    private void OnSetedBomb()
    {
        if (_setting.Bomb > 0 && _bombsInstalled.Count < _setting.Bomb)
        {
            BombInstall();
        }
    }

    private void OnKickedBomb()
    { 
        //kick bomb
    }

    private void BombInstall()
    {
        Bomb newBomb = Instantiate(_template, GetRoundPosition(), _template.transform.rotation);
        _bombsInstalled.Add(newBomb);
        newBomb.Exploded += OnExploded;
    }

    private void OnExploded(Bomb bomb)
    {
        _bombsInstalled.Remove(bomb);
        bomb.Exploded -= OnExploded;
    }

    private Vector3 GetRoundPosition()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.RoundToInt(currentPosition.x);
        currentPosition.y = 0.5f;
        currentPosition.z = Mathf.RoundToInt(currentPosition.z);
        return currentPosition;
    }
}
