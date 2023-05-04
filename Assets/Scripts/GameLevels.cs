using UnityEngine;

public class GameLevels : MonoBehaviour
{
    [SerializeField] private LevelSetting[] _levels;

    public LevelSetting GetLevelSetting(int levelIndex)
    {
        return _levels[levelIndex];
    }
}
