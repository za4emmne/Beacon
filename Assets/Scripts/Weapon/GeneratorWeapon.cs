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
        ResetWeaponPhysics(weapon);
        weapon.transform.position = PositionGeneraton();
        weapon.gameObject.SetActive(true);
        weapon.InitGenerator(this);
        weapon.Initialize();

        return weapon;
    }

    public override void PutObject(Weapon obj)
    {
        ResetWeaponPhysics(obj);
        obj.SetZeroDirection();
        base.PutObject(obj);
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

    private void ResetWeaponPhysics(Weapon weapon)
    {
        // Проверяем, есть ли Rigidbody2D
        Rigidbody2D rb = weapon.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            // Опционально: сбрасываем вращение
            weapon.transform.rotation = Quaternion.identity;
        }
    }
}
