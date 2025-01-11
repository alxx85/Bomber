using System;
using System.Collections;
using UnityEngine;

public abstract class Bosses : Characters
{
    [SerializeField] protected int startActionDelay = 10;

    protected int actionTimer = 0;
    protected bool isActiveAction = false;

    private BossStatsViewer _bossStats;
    private bool _isAlive = true;
    private int _maxHealth;
    private Coroutine _timerCoroutine;
    private WaitForSeconds _timer = new WaitForSeconds(1);

    public virtual event Action<int, int> ChangedTimer;
    public event Action<int, int> ChangedHealth;

    private void Start()
    {
        _bossStats = GameSettings.Instance.BossStatsPanel;
        _bossStats.gameObject.SetActive(true);
        _bossStats.Init(this);
        _maxHealth = health;
        TimerTick();
        ChangedHealth?.Invoke(health, _maxHealth);
        _timerCoroutine = StartCoroutine(StartTimer());
    }

    protected virtual void FixedUpdate()
    {
        //actionTimer += Time.fixedDeltaTime;

        if (isActiveAction == false)
        {
            if (actionTimer >= startActionDelay)
            {
                isActiveAction = true;
                ChangeAction();
            }
        }
    }

    public override void TakeDamage(AttackType attackedOf)
    {
        base.TakeDamage(attackedOf);
        ChangedHealth?.Invoke(health, _maxHealth);
    }

    public override void Died()
    {
        _isAlive = false;
        StopCoroutine(_timerCoroutine);
        _bossStats.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    protected virtual void ChangeAction()
    {
        actionTimer = 0;
    }

    protected virtual void TimerTick()
    {
        ChangedTimer?.Invoke(actionTimer, startActionDelay);
    }

    private IEnumerator StartTimer()
    {
        while (_isAlive)
        {
            yield return _timer;
            actionTimer++;
            TimerTick();
        }
    }
}
