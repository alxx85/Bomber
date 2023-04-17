using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelSetting
{
    public LevelBoost[] LevelBoost;

    public int Level;// { get; private set; }
    public int Width;// { get; private set; }
    public int Height;// { get; private set; }
    public int BrickBlockAmount;// { get; private set; }
    public GameObject BrickBlock;
    public GameObject StoneBlock;
    public List<LevelEnemy> Enemys;// { get; private set; }
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