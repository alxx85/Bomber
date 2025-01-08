using UnityEngine;

public class PlayerStatsViewer : MonoBehaviour
{
    [SerializeField] private PlayerStatPresenter _life;
    [SerializeField] private PlayerStatPresenter _speed;
    [SerializeField] private PlayerStatPresenter _bombAmount;
    [SerializeField] private PlayerStatPresenter _bombPower;

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
