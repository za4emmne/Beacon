using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolObject<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] private float _minPostionX;
    [SerializeField] private float _maxPostionX;
    [SerializeField] private float _minPositionZ;
    [SerializeField] private float _maxPositionZ;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;
    [SerializeField] private float _minDelaySpawn;
    [SerializeField] private float _maxDelaySpawn;
    [SerializeField] private int _maxObjectsInScene;
    [SerializeField] private int _minObjectsInScene;
    [SerializeField] private Transform[] _spawnPoints;

    public int MinObjectInScene => _minObjectsInScene;
    public List<T> ActiveObject => _activeObject;

    private ObjectPool<T> _objectPool;
    private List<T> _activeObject;
    private Coroutine _spawnCoroutine;

    private void Awake()
    {
        _activeObject = new List<T>();
        _objectPool = new ObjectPool<T>
        (
            createFunc: () => Create(GetRandomPosition()), //действия при создании объекта
            actionOnGet: (spawnObject) => OnGet(spawnObject), //действия при взятии свободного объекта из пула
            actionOnRelease: (spawnObject) => OnRelease(spawnObject), //действия при возвращении объекта в пул
            actionOnDestroy: (spawnerObject) => Delete(spawnerObject), //действия при удалении объекта из пула
            collectionCheck: true, //необходимо ли проверять коллекцию при возвращении в пул, работает только в редакторе
            defaultCapacity: _poolCapacity, //размер пула по умолчанию
            maxSize: _poolMaxSize //максимальный размер пула
        );
    }

    public virtual void StartGeneration()
    {
        if (_spawnCoroutine == null)
            _spawnCoroutine = StartCoroutine(SpawnWithDelay());
    }

    public virtual T Create(Vector3 vector3)
    {
        T spawnObject = Instantiate(_prefab, vector3, Quaternion.identity);

        return spawnObject;
    }

    public virtual void OnGet(T spawnObject)
    {
        spawnObject.gameObject.SetActive(true);
        _activeObject.Add(spawnObject);
    }

    public virtual void OnRelease(T spawnObject)
    {
        spawnObject.gameObject.SetActive(false);
        _activeObject.Remove(spawnObject);
    }

    public virtual void Delete(T spawnObject)
    {
        Destroy(spawnObject.gameObject);
    }

    protected Vector3 GetRandomPosition()
    {
        int randomPoint = Random.Range(0, _spawnPoints.Length);

        return new Vector3(_spawnPoints[randomPoint].position.x, _spawnPoints[randomPoint].position.y, _spawnPoints[randomPoint].position.z);

        //float randomPositionX = Random.Range(_minPostionX, _maxPostionX);
        //float randomPositionZ = Random.Range(_minPositionZ, _maxPositionZ);

        //return new Vector3(randomPositionX, 0, randomPositionZ);
    }

    private IEnumerator SpawnWithDelay()
    {
        float randomDelaySpawn = Random.Range(_minDelaySpawn, _maxDelaySpawn);
        WaitForSeconds waitSpawn = new WaitForSeconds(randomDelaySpawn);

        while (_activeObject.Count < _maxObjectsInScene)
        {
            _objectPool.Get();
            yield return waitSpawn;
        }

        _spawnCoroutine = null;
    }
}
