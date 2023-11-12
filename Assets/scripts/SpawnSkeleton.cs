using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSkeleton : MonoBehaviour
{
    //[SerializeField] private Transform _spawnPlace;
    [SerializeField] private Enemy _template;
    [SerializeField] private int _delay = 2;
    private Target _target;


    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
        _target = GetComponentInChildren<Target>();
    }

    private IEnumerator SpawnCoroutine()
    {
        var waitForAnySecond = new WaitForSeconds(_delay);

        while (true)
        {
            Enemy _enemy = Instantiate(_template, transform.position, Quaternion.identity);

            yield return waitForAnySecond;
        }
    }

    public Transform GetTarget()
    {
        return _target.transform;
    }
}