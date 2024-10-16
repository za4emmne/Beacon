using System.Collections;
using UnityEngine;

public class SpawnerEnemy : PoolObject<EnemyViewer>
{
    private void Start()
    {
        base.StartGeneration();
    }
}