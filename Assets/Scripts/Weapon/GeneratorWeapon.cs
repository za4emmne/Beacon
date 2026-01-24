using System.Collections.Generic;
using UnityEngine;

public class GeneratorWeapon : Spawner<Weapon>
{
    [SerializeField] private int _projectilesPerShot = 1;
    [SerializeField] private float _knifeSpreadAngle = 15f;
    [SerializeField] private float _swordSpreadAngle = 25f;     // Дуга для мечей
    [SerializeField] private float _swordOffsetDistance = 0.4f; // Смещение от игрока

    private Coroutine _secondCoroutine;
    private static List<Weapon> _allProjectile = new List<Weapon>();

    private void Start()
    {
        //base.OnStartGenerator();
    }

    public void InitSpawnDelay(float minDataDelay, float maxDataDelay)
    {
        minDelay = minDataDelay;
        maxDelay = maxDataDelay;
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

    public void SetProjectilesPerShot(int count)
    {
        _projectilesPerShot = count;
    }

    public void SpawnProjectilesBurst()
    {
        // базовое направление – как в KnifeBehavior
        Vector2 baseDir = GetPlayerDirection();

        int count = _projectilesPerShot;
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

        if (_projectilesPerShot == 1)
        {
            SpawnOneProjectilesWithAngle(baseAngle);
            return;
        }

        float totalSpread = _knifeSpreadAngle * (count - 1);
        float startAngle = baseAngle - totalSpread / 2f;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + _knifeSpreadAngle * i;
            SpawnOneProjectilesWithAngle(angle);
        }
    }

    private void SpawnOneProjectilesWithAngle(float angle)
    {
        Weapon weapon = base.GetObject();

        if (weapon == null)
            return;

        ResetWeaponPhysics(weapon);
        weapon.transform.position = PositionGeneraton();
        weapon.gameObject.SetActive(true);
        weapon.InitGenerator(this);

        float rad = angle * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        if (weapon is KnifeBehavior knife)
            knife.InitializeWithDirection(dir);
        else
            weapon.Initialize();
    }

    public void SetSwordSpreadAngle(float angle)
    {
        _swordSpreadAngle = angle;
    }

    // Переопределяем burst для мечей
    public void SpawnSwordBurst()
    {
        Vector2 baseDir = GetPlayerDirection();
        int count = _projectilesPerShot;
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;

        if (count == 1)
        {
            SpawnSingleSword(baseAngle);
            return;
        }

        // Распределяем мечи по дуге
        float totalSpread = _swordSpreadAngle * (count - 1);
        float startAngle = baseAngle - totalSpread / 2f;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + _swordSpreadAngle * i;
            SpawnSingleSword(angle);
        }
    }

    private void SpawnSingleSword(float angle)
    {
        SwordBehavior sword = base.GetObject() as SwordBehavior;
        if (sword == null) return;

        ResetWeaponPhysics(sword);

        // Позиционируем меч со смещением от игрока
        Vector2 offsetDir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                                       Mathf.Sin(angle * Mathf.Deg2Rad));
        sword.transform.position = PositionGeneraton() + (Vector3)(offsetDir * _swordOffsetDistance);

        sword.gameObject.SetActive(true);
        sword.InitGenerator(this);
        sword.Initialize();
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

            weapon.transform.rotation = Quaternion.identity;
        }
    }

    private Vector2 GetPlayerDirection()
    {
        if (Player.singleton == null) return Vector2.right;

        var playerMovement = Player.singleton.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            if (playerMovement.MovementDirection.sqrMagnitude > 0.001f)
                return playerMovement.MovementDirection.normalized;
            if (playerMovement.LastMoveDirection.sqrMagnitude > 0.001f)
                return playerMovement.LastMoveDirection.normalized;
        }
        return Vector2.right;
    }
}
