using System.Collections.Generic;
using UnityEngine;

public class GeneratorWeapon : Spawner<Weapon>
{
    private Coroutine _secondCoroutine;
    private static List<Weapon> _allProjectile = new List<Weapon>();

    private void Start()
    {
        base.OnStartGenerator();
    }

    public void ChangedSpawnDelay(int level)
    {
        minDelay /= level;
        maxDelay /= level;
        maxDelay *= 1.6f;
    }

    public void OnStartSecondGeneration()
    {
        if (_secondCoroutine == null)
        {
            _secondCoroutine = StartCoroutine(Spawn());
        }
    }

    public void AddProjectileOnList(Weapon weapon)
    {
        _allProjectile.Add(weapon);
    }

    public void RemoveProjectileFromList(Weapon weapon)
    {
        _allProjectile.Remove(weapon);
    }

    public override Weapon GetObject()
    {
        var weapon = base.GetObject();

        weapon.InitGenerator(this);
        weapon.Initialize();

        return weapon;
    }

    public override void PutObject(Weapon obj)
    {
        base.PutObject(obj);
        obj.SetZeroDirection();
    }

    public Enemy FindNearestEnemy()
    {
        Enemy closest = null;
        float minDist = Mathf.Infinity;
        float maxDist = 8f;
        Vector3 pos = transform.position;

        if (EnemiesGenerator.AllEnemies.Count > 0)
        {
            for (int i = 0; i < EnemiesGenerator.AllEnemies.Count; i++)
            {
                Enemy enemy = EnemiesGenerator.AllEnemies[i];
                float dist = (enemy.transform.position - pos).sqrMagnitude;

                if (dist < minDist && dist < maxDist)
                {
                    minDist = dist;
                    closest = enemy;
                    enemy.RemoveFromList();
                }
            }
        }

        return closest;
    }

    protected override Vector3 PositionGeneraton()
    {
        return _transform.position;
    }
}
