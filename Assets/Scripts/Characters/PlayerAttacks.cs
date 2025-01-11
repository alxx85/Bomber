//using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private Bomb _template;
    [SerializeField] private LayerMask _destroyedMask;

    private List<Bomb> _bombsInstalled = new List<Bomb>();
    private GameSettings _setting;
    private PlayerInput _input;
    private PlayerSFX _audio;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _audio = GetComponent<PlayerSFX>();
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
                Destroy(bomb.gameObject);
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
            _audio.Play(AudioName.SetBomb);
            BombInstall();
        }
    }

    private void OnKickedBomb()
    { 
        //Hand activate bomb
        if (_setting.CanControl && _bombsInstalled.Count > 0)
        {
            _bombsInstalled[0].Activate();
        }
    }

    private void BombInstall()
    {
        Bomb newBomb = Instantiate(_template, GetRoundPosition(), _template.transform.rotation);
        _bombsInstalled.Add(newBomb);
        newBomb.Init();
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
