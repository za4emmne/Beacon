using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class LightningWeapon : Weapon
{
    [Header("Lightning Settings")]
    [SerializeField] private float strikeRadius = 15f;
    [SerializeField] private int maxChainTargets = 3;
    [SerializeField] private float chainRadius = 5f;
    [SerializeField] private float chainDelay = 0.1f;

    [Header("Visual Effects")]
    [SerializeField] private LineRenderer lightningPrefab;
    [SerializeField] private GameObject impactEffectPrefab;
    [SerializeField] private float lightningDuration = 0.2f;
    [SerializeField] private AudioClip lightningSound;

    private AudioSource _audioSource;
    private List<Enemy> hitEnemies = new List<Enemy>();
    private LightningGenerator lightningGenerator;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();
    }

    // НОВЫЙ МЕТОД для связи с LightningGenerator
    public void InitGenerator(LightningGenerator generator)
    {
        lightningGenerator = generator;
        // НЕ вызываем base.InitGenerator, потому что это другой тип генератора
    }

    public override void Initialize()
    {
        // Вызываем базовую инициализацию
        base.Initialize();

        // Получаем параметры из data (если есть)
        if (data != null)
        {
            strikeRadius = data.CurrentAttackRange;
            maxChainTargets = Mathf.RoundToInt(data.CurrentSpeed);
            damage = data.CurrentDamage;
        }

        Strike();
    }

    private void Strike()
    {
        Enemy target = FindRandomEnemy(transform.position, strikeRadius);

        if (target != null)
        {
            hitEnemies.Clear();
            StartCoroutine(ChainLightning(transform.position, target, 0));
        }
        else
        {
            ReturnToPool();
        }
    }

    private IEnumerator ChainLightning(Vector3 startPos, Enemy target, int chainCount)
    {
        if (target == null || chainCount >= maxChainTargets)
        {
            ReturnToPool();
            yield break;
        }

        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        hitEnemies.Add(target);
        CreateLightningEffect(startPos, target.transform.position);

        if (impactEffectPrefab != null)
        {
            GameObject impact = Instantiate(impactEffectPrefab, target.transform.position, Quaternion.identity);
            Destroy(impact, 1f);
        }

        if (lightningSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(lightningSound);
        }

        yield return new WaitForSeconds(chainDelay);

        Enemy nextTarget = FindNextChainTarget(target.transform.position);

        if (nextTarget != null)
        {
            StartCoroutine(ChainLightning(target.transform.position, nextTarget, chainCount + 1));
        }
        else
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        // Используем специальный генератор молний
        if (lightningGenerator != null)
        {
            lightningGenerator.PutObject(this);
        }
        else
        {
            // Fallback: просто деактивируем
            gameObject.SetActive(false);
        }
    }

    private void CreateLightningEffect(Vector3 start, Vector3 end)
    {
        if (lightningPrefab == null) return;

        LineRenderer lightning = Instantiate(lightningPrefab);
        lightning.positionCount = 3;

        lightning.startWidth = 0.05f;
        lightning.endWidth = 0.02f;

        Vector3[] positions = GenerateLightningPath(start, end, lightning.positionCount);
        lightning.SetPositions(positions);

        Destroy(lightning.gameObject, lightningDuration);
    }

    private Vector3[] GenerateLightningPath(Vector3 start, Vector3 end, int segments)
    {
        Vector3[] positions = new Vector3[segments];
        positions[0] = start;
        positions[segments - 1] = end;

        Vector3 direction = (end - start) / (segments - 1);

        for (int i = 1; i < segments - 1; i++)
        {
            Vector3 basePos = start + direction * i;
            Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward).normalized;
            float offset = Random.Range(-0.2f, 0.2f);
            positions[i] = basePos + perpendicular * offset;
        }

        return positions;
    }

    private Enemy FindRandomEnemy(Vector3 center, float radius)
    {
        if (EnemiesGenerator.AllEnemies == null || EnemiesGenerator.AllEnemies.Count == 0)
            return null;

        List<Enemy> enemiesInRange = new List<Enemy>();

        foreach (Enemy enemy in EnemiesGenerator.AllEnemies)
        {
            if (enemy != null && Vector3.Distance(center, enemy.transform.position) <= radius)
            {
                enemiesInRange.Add(enemy);
            }
        }

        if (enemiesInRange.Count > 0)
        {
            return enemiesInRange[Random.Range(0, enemiesInRange.Count)];
        }

        return null;
    }

    private Enemy FindNextChainTarget(Vector3 center)
    {
        if (EnemiesGenerator.AllEnemies == null || EnemiesGenerator.AllEnemies.Count == 0)
            return null;

        Enemy closest = null;
        float minDist = float.MaxValue;

        foreach (Enemy enemy in EnemiesGenerator.AllEnemies)
        {
            if (enemy != null && !hitEnemies.Contains(enemy))
            {
                float dist = Vector3.Distance(center, enemy.transform.position);
                if (dist <= chainRadius && dist < minDist)
                {
                    minDist = dist;
                    closest = enemy;
                }
            }
        }

        return closest;
    }

    // Переопределяем OnTriggerEnter2D — молния не использует коллизии
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // Молния не реагирует на физические коллизии
    }
}
