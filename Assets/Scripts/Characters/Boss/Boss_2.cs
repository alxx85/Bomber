using System.Collections;
using UnityEngine;

public class Boss_2 : Bosses
{
    [SerializeField] private Enemy _template;

    private bool _canSpawn = true;

    protected override void ChangeAction()
    {
        StartCoroutine(SpawnEnemy());
        _actionTimer = 0;
    }

    private IEnumerator SpawnEnemy()
    {
        if (_template != null && _canSpawn)
        {
            _canSpawn = false;
            Enemy enemy = Instantiate(_template, transform.position, Quaternion.identity);
            enemy.gameObject.SetActive(false);
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
