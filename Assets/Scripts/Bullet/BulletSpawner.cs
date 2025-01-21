using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : PoolObject<Bullet>
{
    private void Start()
    {
        base.StartGeneration();
    }
    protected override Vector3 GetRandomPosition()
    {
        return transform.position;
    }
}
