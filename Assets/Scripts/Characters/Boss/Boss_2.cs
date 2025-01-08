using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_2 : Bosses
{
    [SerializeField] private List<Enemy> _templates;
    [SerializeField] private int _addChangedDelay = 0;
    [SerializeField] private int _changedDelayByCount = 3;

    private bool _canSpawn = true;
    private int _spawnCount = 0;

    protected override void ChangeAction()
    {
        StartCoroutine(SpawnEnemy());
        actionTimer = 0;
        isActiveAction = false;

        if (_changedDelayByCount > 0 && _spawnCount >= _changedDelayByCount)
        {
            startActionDelay += _addChangedDelay;
            _spawnCount = 0;
        }
    }

    private IEnumerator SpawnEnemy()
    {
        if (_templates.Count > 0 && _canSpawn)
        {
            _canSpawn = false;
            int spawnIndex = Random.Range(0, _templates.Count);
            Enemy enemy = Instantiate(_templates[spawnIndex], transform.position, Quaternion.identity);
            enemy.gameObject.SetActive(false);
            GameSettings.Instance.AddEnemyOnList(enemy);
            _spawnCount++;
            yield return new WaitForSeconds(1f);

            enemy.gameObject.SetActive(true);
            _canSpawn = true;
        }
        else
        {
            yield return null;
        }
    }
}
