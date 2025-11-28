using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T[] _objects;
    [SerializeField] protected float minDelay;
    [SerializeField] protected float maxDelay;
    [SerializeField] private float _devationPositionY;
    [SerializeField] private float _devationPositionX;

    protected Transform _transform;
    protected EnemiesGenerator _enemyGenerator;

    [Header("Мониторинг данных")]
    [SerializeField] private float _spawnTime;
    private Coroutine _coroutine;

    protected Queue<T> pool;

    public IEnumerable<T> PooledObjects => pool;

    protected virtual void Awake()
    {
        pool = new Queue<T>();
        _transform = GetComponent<Transform>();
    }

    public virtual T GetObject()
    {
        if (pool.Count == 0)
        {
            var obj = Instantiate(_objects[0/*index*/]);

            return obj;
        }

        return pool.Dequeue();
    }

    public virtual void PutObject(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }

    protected virtual Vector3 PositionGeneraton()
    {
        float minPositionY = transform.position.y - _devationPositionY;
        float maxPositionY = transform.position.y + _devationPositionY;
        float minPositionX = transform.position.x - _devationPositionX;
        float maxPositionX = transform.position.x + _devationPositionX;
        float positionY = Random.Range(minPositionY, maxPositionY);
        float positionX = Random.Range(minPositionX, maxPositionX);

        Vector3 spawnPoint = new Vector3(positionX, positionY, transform.position.z);

        return spawnPoint;
    }

    public void Reset()
    {
        pool.Clear();
    }

    public virtual void OnStartGenerator()
    {
        if (_coroutine != null)
        {
            Debug.LogWarning("Spawn coroutine already running!");
            return;
        }

        _coroutine = StartCoroutine(Spawn());
    }

    public virtual void OnStop()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    public void OneSpawn()
    {
        int count = Random.Range(0, 3);

        for(int i = 0; i < count; i++)
        {
            var obj = GetObject();
            obj.gameObject.SetActive(true);
            obj.transform.position = PositionGeneraton();
        }
    }

    protected IEnumerator Spawn()
    {
        while (true)
        {
            _spawnTime = Random.Range(minDelay, maxDelay);
            //Debug.Log($"Spawning in {_spawnTime} seconds...");

            var waitForSeconds = new WaitForSeconds(_spawnTime);

            var obj = GetObject();

            if (obj != null)
            {
                obj.gameObject.SetActive(true);
                obj.transform.position = PositionGeneraton();
                //Debug.Log($"Object spawned: {obj.name}");
            }
            else
            {
                Debug.LogWarning("GetObject returned NULL!");
            }

            yield return waitForSeconds;
        }
    }

}
