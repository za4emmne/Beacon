using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyAttacked))]
public class Leshy : Enemy
{
    [Header("References")]
    [SerializeField] private LeshyData _data;
    [SerializeField] private Transform _batSpawnPoint;
    
    private EnemiesGenerator _enemyGenerator;
    private Transform _playerTransform;
    
    private bool _isDead = false;
    private bool _canAttack = true;
    private Coroutine _attackCoroutine;
    
    protected override void Awake()
    {
        base.Awake();
        
        if (_batSpawnPoint == null)
            _batSpawnPoint = transform;
    }
    
    public void Initialize(LeshyData data, EnemiesGenerator enemyGenerator, Transform playerTransform)
    {
        if (data == null)
        {
            Debug.LogError("LeshyData is null!");
            return;
        }
        
        // Initialize base Enemy components
        base.Initialize(data, enemyGenerator);
        
        // Override speed to 0 for stationary behavior after base initialization
        _speed = 0f;
        
        _enemyGenerator = enemyGenerator;
        _playerTransform = playerTransform;
        
        // Start attack cycle
        StartAttackCycle();
    }
    
    private void StartAttackCycle()
    {
        if (_isDead) return;
        
        // Cancel any existing coroutine
        if (_attackCoroutine != null)
            StopCoroutine(_attackCoroutine);
            
        // Start new attack coroutine
        _attackCoroutine = StartCoroutine(AttackRoutine());
    }
    
    private IEnumerator AttackRoutine()
    {
        while (!_isDead && _canAttack)
        {
            // Perform bat attack
            yield return StartCoroutine(SpawnBats());
            
            // Wait for cooldown before next attack
            _canAttack = false;
            yield return new WaitForSeconds(_data.AttackCooldown);
            _canAttack = true;
        }
    }
    
    private IEnumerator SpawnBats()
    {
        if (_data.BatData == null)
        {
            Debug.LogWarning("Bat data not assigned to Leshy!");
            yield break;
        }
        
        for (int i = 0; i < _data.BatsPerAttack; i++)
        {
            if (_isDead) yield break;
            
            // Spawn bat using the enemy generator with EnemyData
            Enemy batEnemy = _enemyGenerator.GetEnemyFromPool(_data.BatData);
            
            if (batEnemy != null)
            {
                batEnemy.Initialize(_data.BatData, _enemyGenerator);
                batEnemy.gameObject.SetActive(true);
                
                // Position the bat at the spawn point
                Vector3 spawnPos = _batSpawnPoint.position;
                // Add small random offset to prevent overlap
                spawnPos.x += Random.Range(-0.5f, 0.5f);
                spawnPos.y += Random.Range(-0.5f, 0.5f);
                batEnemy.transform.position = spawnPos;
                
                // Set bat to fly toward player (assuming bat movement is handled by its own components)
            }
            
            // Wait between bat spawns
            if (i < _data.BatsPerAttack - 1) // Don't wait after the last bat
                yield return new WaitForSeconds(_data.BatSpawnDelay);
        }
    }
    
    public void OnDeath()
    {
        if (_isDead) return;
        _isDead = true;
        
        // Stop attack coroutine
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
        
        // Remove from enemy list
        _enemyGenerator.RemoveEnemyFromList(this);
        
        // Return to pool
        _enemyGenerator.ReturnEnemyToPool(this);
    }
    
    private void OnDisable()
    {
        // Cleanup when disabled
        if (_enemyGenerator != null && !_isDead)
        {
            _enemyGenerator.RemoveEnemyFromList(this);
            _enemyGenerator.ReturnEnemyToPool(this);
        }
        
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
    }
}