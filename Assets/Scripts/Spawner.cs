using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _object;
    [SerializeField] private float _minDelay;
    [SerializeField] private float _maxDelay;
    [SerializeField] private float _devationPositionY;
    [SerializeField] private float _devationPositionX;
    private Transform _transform;

    [Header("Мониторинг данных")]
    [SerializeField] private float _spawnTime;
    private Coroutine _coroutine;

    private Queue<T> _pool;

    public IEnumerable<T> PooledObjects => _pool;

    private void Awake()
    {
        _pool = new Queue<T>();
        _transform = GetComponent<Transform>();
    }

    private void Start()
    {

    }

    public virtual T GetObject()
    {
        if (_pool.Count == 0)
        {
            var obj = Instantiate(_object);

            return obj;
        }

        return _pool.Dequeue();
    }

    public virtual void PutObject(T obj)
    {
        _pool.Enqueue(obj);
        obj.gameObject.SetActive(false);
    }

    public void Reset()
    {
        _pool.Clear();
    }

    public void OnStartGenerator()
    {
        if (_coroutine == null)
        {
            _coroutine = StartCoroutine(Spawn());
        }

    }

    private void OnStop()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    private IEnumerator Spawn()
    {
        float minPositionY = transform.position.y - _devationPositionY;
        float maxPositionY = transform.position.y + _devationPositionY;
        float minPositionX = transform.position.x - _devationPositionX;
        float maxPositionX = transform.position.x + _devationPositionX;

        while (true)
        {

            float positionY = Random.Range(minPositionY, maxPositionY);
            float positionX = Random.Range(minPositionX, maxPositionX);
            _spawnTime = Random.Range(_minDelay, _maxDelay);

            var waitForSeconds = new WaitForSeconds(_spawnTime);

            Vector3 spawnPoint = new Vector3(positionX, positionY, transform.position.z);
            var obj = GetObject();

            obj.gameObject.SetActive(true);
            obj.transform.parent = transform;
            obj.transform.position = spawnPoint;

            yield return waitForSeconds;
        }
    }
}
