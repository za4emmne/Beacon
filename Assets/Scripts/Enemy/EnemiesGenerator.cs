using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGenerator : MonoBehaviour
{
    [SerializeField] private StarGenerator _starGenerator;
    [SerializeField] protected Enemy[] _objects;
    [SerializeField] protected float minDelay;
    [SerializeField] protected float maxDelay;
    [SerializeField] private float _devationPositionY;
    [SerializeField] private float _devationPositionX;

    private Transform _transform;

    [Header("Мониторинг данных")]
    [SerializeField] private float _spawnTime;
    private Coroutine _coroutine;

    protected Queue<Enemy> pool;

    public IEnumerable<Enemy> PooledObjects => pool;
    public event Action OneKill;

    protected virtual void Awake()
    {
        pool = new Queue<Enemy>();
        _transform = GetComponent<Transform>();
    }

    public Enemy GetObject(EnemyData data)
    {
        if (pool.Count == 0)
        {
            var enemy = Instantiate(_objects[0]); //необходимо автоматически выбирать преаб врага в зависимости от волн
            enemy.Initialize(data, this);
            enemy.transform.parent = transform;
 
            return enemy;
        }

        return pool.Dequeue();
    }

    public void PutObject(Enemy enemy)
    {
        OneKill?.Invoke();
        var star = _starGenerator.GetObject();
        star.transform.position = enemy.transform.position;

        pool.Enqueue(enemy);
        enemy.gameObject.SetActive(false);
    }

    protected virtual Vector3 PositionGeneraton()
    {
        float minPositionY = transform.position.y - _devationPositionY;
        float maxPositionY = transform.position.y + _devationPositionY;
        float minPositionX = transform.position.x - _devationPositionX;
        float maxPositionX = transform.position.x + _devationPositionX;
        float positionY = UnityEngine.Random.Range(minPositionY, maxPositionY);
        float positionX = UnityEngine.Random.Range(minPositionX, maxPositionX);

        Vector3 spawnPoint = new Vector3(positionX, positionY, transform.position.z);

        return spawnPoint;
    }

    public void SpawnEnemyWithModifiers(EnemyData baseData, int waveIndex)
    {
        Enemy enemy = GetObject(baseData);//????
        enemy.Initialize(baseData, this);
        enemy.gameObject.SetActive(true);
        enemy.transform.position = PositionGeneraton();
    }

    public void Reset()
    {
        pool.Clear();
    }
}