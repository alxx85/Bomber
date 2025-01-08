using UnityEngine;

public class LevelStatsViewer : MonoBehaviour
{
    [SerializeField] private BarPresenter _timeBar;
    [SerializeField] private TextPresenter _levelTextField;
    [SerializeField] private TextPresenter _enemysTextField;

    private GameSettings _settings;
    private float _currentValue;

    private void Start()
    {
        _settings = GameSettings.Instance;
        _settings.ChangedLevelTime += OnChangedTime;
        _settings.ChangedEnemyCount += OnChangedEnemyCount;
        _levelTextField.Show((_settings.GetLevelNumber() + 1).ToString());
    }

    private void OnDisable()
    {
        _settings.ChangedLevelTime -= OnChangedTime;
        _settings.ChangedEnemyCount -= OnChangedEnemyCount;
    }

    private void OnChangedTime(int current, int maxTime)
    {
        _currentValue = (float)current / (float)maxTime;
        _timeBar.Show(_currentValue);
    }
    
    private void OnChangedEnemyCount(int count)
    {
        _enemysTextField.Show(count.ToString());
    }
}
