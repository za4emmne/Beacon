using System.Collections;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    [SerializeField] private Enemy _template;
    [SerializeField] private int _delay = 2;

    private int _countEnemies = 10;

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        var waitForAnySecond = new WaitForSeconds(_delay);

        for(int i = 0; i<_countEnemies; i++)
        {
            Enemy _enemy = Instantiate(_template, transform.position, Quaternion.identity);

            yield return waitForAnySecond;
        }
    }
}