using UnityEngine;

public class StatsViewer : MonoBehaviour
{
    [SerializeField] private StatView _life;
    [SerializeField] private StatView _speed;
    [SerializeField] private StatView _bombAmount;
    [SerializeField] private StatView _bombPower;

    private GameSettings _settings;

    private void Start()
    {
        _settings = GameSettings.Instance;
        _settings.ChangedPlayerProperties += OnChangedStat;
        OnChangedStat();
    }

    private void OnDisable()
    {
        _settings.ChangedPlayerProperties -= OnChangedStat;
    }

    private void OnChangedStat()
    {
        _life.ChangeStat(_settings.Lifes);
        _speed.ChangeStat(_settings.SpeedLevel);
        _bombAmount.ChangeStat(_settings.Bomb);
        _bombPower.ChangeStat(_settings.Power);
    }
}
