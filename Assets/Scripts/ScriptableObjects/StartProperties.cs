using UnityEngine;

[CreateAssetMenu(fileName = "NewGameProperties",menuName ="Properties/NewGamePropertie")]
public class StartProperties : ScriptableObject
{
    [SerializeField] private int _currentLevel = 0;
    [SerializeField] private int _lifes = 3;
    [SerializeField] private float _speed = 3;
    [SerializeField] private int _bombAmount = 1;
    [SerializeField] private int _bombPower = 1;
    [SerializeField] private bool _canActivateControlBomb = false;
    [SerializeField] private bool _useShield = false;

    public SavedProperties GetSavedProperties(string profileName)
    {
        LoadProfileProperties(profileName);

        return new SavedProperties(_currentLevel, _lifes, _speed, _bombAmount, _bombPower, 
                                    _canActivateControlBomb, _useShield);
    }

    public void SaveProfile(string profileName, SavedProperties properties, InputSettings profileKeys)
    {
        SaverPlayers.SavePlayer(profileName, properties, profileKeys);
    }

    private void LoadProfileProperties(string name)
    {
        var loadedProperties = SaverPlayers.LoadPlayer(name);

        if (loadedProperties == null)
            return;

        _currentLevel = loadedProperties.Level;
        _lifes = loadedProperties.Life;
        _speed = loadedProperties.Speed;
        _bombAmount = loadedProperties.BombAmount;
        _bombPower = loadedProperties.BombPower;
        _canActivateControlBomb = loadedProperties.CanActivateControlBomb;
        _useShield = loadedProperties.UseShield;
    }
}

public class SavedProperties
{
    public int Level { get; private set; }
    public int Life { get; private set; }
    public float Speed { get; private set; }
    public int BombAmount { get; private set; }
    public int BombPower { get; private set; }
    public bool CanActivateControlBomb { get; private set; }
    public bool UseShield { get; private set; }

    public SavedProperties(int level, int life, float speed, int bombAmount, int bombPower, bool canActivateControlBomb, bool useShield)
    {
        Level = level;
        Life = life;
        Speed = speed;
        BombAmount = bombAmount;
        BombPower = bombPower;
        CanActivateControlBomb = canActivateControlBomb;
        UseShield = useShield;
    }
}