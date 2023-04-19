using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Level", menuName ="Levels/AddLevel", order = 50)]
public class LevelSetting : ScriptableObject
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private int _brickBlockAmount;
    [SerializeField] private GameObject _brickBlock;
    [SerializeField] private GameObject _stoneBlock;
    [SerializeField] private LevelBoost[] _levelBoost;
    [SerializeField] private List<LevelEnemy> _enemys;

    public int Width => _width;
    public int Height => _height;
    public int BrickBlockAmount => _brickBlockAmount;
    public GameObject BrickBlock => _brickBlock;
    public GameObject StoneBlock => _stoneBlock;
    public LevelBoost[] LevelBoost => _levelBoost;
    public List<LevelEnemy> Enemys => _enemys;
}

public class Boost
{
    public bool Life { get; private set; }
    public bool Speed { get; private set; }
    public bool BombAmount { get; private set; }
    public bool BombPower { get; private set; }
    public bool Kick { get; private set; }
    public bool Shield { get; private set; }

    public Boost(bool life, bool speed, bool amount, bool power, bool kick, bool shield)
    {
        Life = life;
        Speed = speed;
        BombAmount = amount;
        BombPower = power;
        Kick = kick;
        Shield = shield;
    }
}

[Serializable]
public class LevelBoost
{
    public Booster Booster;
    public int Amount;
}

[Serializable]
public class LevelEnemy
{
    public Character Enemy;
    public int Amount;
}