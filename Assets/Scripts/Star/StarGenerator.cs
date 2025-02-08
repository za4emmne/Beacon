using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGenerator : PoolObject<Star>
{
    [SerializeField] private EnemyManager _enemyManager;

    private void Start()
    {
        base.StartGeneration();
    }

    public override void OnGet(Star spawnObject)
    {
        base.OnGet(spawnObject);
        spawnObject.Initialize(this);
    }

    protected override Vector3 GetRandomPosition()
    {
        float randomPositionX = Random.Range(_minPostionX, _maxPostionX);
        float randomPositionZ = Random.Range(_minPositionZ, _maxPositionZ);

        return new Vector3(_spawnPoints[0].position.x + randomPositionX, _spawnPoints[0].position.y, _spawnPoints[0].position.z + randomPositionZ);
    }
}
