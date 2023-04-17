using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevels : MonoBehaviour
{
    [SerializeField] private LevelSetting[] _levels;

    public LevelSetting GetLevelSetting(int levelIndex)
    {
        return _levels[levelIndex];
    }
}
