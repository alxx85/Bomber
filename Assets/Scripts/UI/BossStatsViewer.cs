using System;
using UnityEngine;

public class BossStatsViewer : MonoBehaviour
{
    [SerializeField] private BarPresenter _healthBar;
    [SerializeField] private BarPresenter _timeBar;

    private Bosses _boss;

    public void Init(Bosses boss)
    {
        _boss = boss;
        _boss.ChangedHealth += OnChangedHealth;
        _boss.ChangedTimer += OnChangedTimer;
    }

    private void OnChangedTimer(int current, int delay)
    {
        float value = (float)current / (float)delay;
        _timeBar.Show(value);
    }

    private void OnChangedHealth(int current, int maxHealth)
    {
        float value = (float)current / (float)maxHealth;
        _healthBar.Show(value);
    }

    private void OnDisable()
    {
        _boss.ChangedHealth -= OnChangedHealth;
        _boss.ChangedTimer -= OnChangedTimer;

    }
}
