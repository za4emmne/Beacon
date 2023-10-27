using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstantiete : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPlaces = new Transform[3];
    [SerializeField] private Skeleton[] _templates = new Skeleton[1];

    private int _number = 0 ;
    private int _spawnTime = 2;

    private void Start()
    { 
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        var waitForAnySecond = new WaitForSeconds(_spawnTime);

        while (true)
        {
            Skeleton enemy = Instantiate(_templates[Random.Range(0, _templates.Length)], _spawnPlaces[_number].position, Quaternion.identity);
            _number++;
  
            if (_number == _spawnPlaces.Length) 
                _number = 0;

            yield return waitForAnySecond;
        }
    }
}
