using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstantiete : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPlaces = new Transform[3];
    [SerializeField] private Skeleton[] Templates = new Skeleton[1];
    private int _number = 0 ;
    private int _spawnTime = 2;

    private void Start()
    { 
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            Skeleton enemy = Instantiate(Templates[Random.Range(0, Templates.Length)], spawnPlaces[_number].position, Quaternion.identity);
            _number++;
  
            if (_number == spawnPlaces.Length) 
                _number = 0;

            yield return new WaitForSeconds(_spawnTime);
        }
    }
}
