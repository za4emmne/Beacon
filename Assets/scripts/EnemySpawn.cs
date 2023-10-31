using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPlaces;
    [SerializeField] private Enemy[] _templates;
    [SerializeField] private int _delay = 2;

    public Transform Target;
    private int _number = 0;


    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    public void Update()
    {
        Target = GameObject.FindObjectOfType<firstSpawner>().GetTarget();
    }

    private IEnumerator SpawnCoroutine()
    {
        var waitForAnySecond = new WaitForSeconds(_delay);

        while (true)
        {
            Enemy enemy = Instantiate(_templates[Random.Range(0, _templates.Length)], _spawnPlaces[_number].position, Quaternion.identity);
            _number++;

            if (_number == _spawnPlaces.Length)
                _number = 0;

            yield return waitForAnySecond;
        }
    }
}