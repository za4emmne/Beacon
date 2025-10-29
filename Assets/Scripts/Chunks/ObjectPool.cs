using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    private Dictionary<string, Queue<GameObject>> poolDictionary = new();
    private Dictionary<GameObject, string> activeObjects = new(); // ������������ �������� ��������

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �������� ������ �� ����
    /// </summary>
    public GameObject Get(GameObject prefab, Vector3 position, Transform parent = null)
    {
        string key = prefab.name;

        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary[key] = new Queue<GameObject>();
        }

        GameObject obj;

        if (poolDictionary[key].Count > 0)
        {
            obj = poolDictionary[key].Dequeue();
            obj.transform.position = position;
            obj.transform.SetParent(parent);
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefab, position, Quaternion.identity, parent);
            obj.name = prefab.name; // ������� (Clone) �� �����
        }

        activeObjects[obj] = key;
        return obj;
    }

    /// <summary>
    /// ������� ������ � ���
    /// </summary>
    public void Release(GameObject obj)
    {
        if (obj == null) return;

        if (activeObjects.TryGetValue(obj, out string key))
        {
            obj.SetActive(false);
            obj.transform.SetParent(transform); // ���������� ��� ObjectPool
            poolDictionary[key].Enqueue(obj);
            activeObjects.Remove(obj);
        }
        else
        {
            Debug.LogWarning($"Object {obj.name} not found in pool, destroying");
            Destroy(obj);
        }
    }

    /// <summary>
    /// ������� ��� �������� ������� �������� � ���
    /// </summary>
    public void ReleaseChildren(Transform parent)
    {
        List<GameObject> toRelease = new();

        foreach (Transform child in parent)
        {
            if (activeObjects.ContainsKey(child.gameObject))
            {
                toRelease.Add(child.gameObject);
            }
        }

        foreach (var obj in toRelease)
        {
            Release(obj);
        }
    }

    /// <summary>
    /// ��������������� ������� ����
    /// </summary>
    public void Prewarm(GameObject prefab, int count)
    {
        string key = prefab.name;
        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary[key] = new Queue<GameObject>();
        }

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.name = prefab.name;
            obj.SetActive(false);
            poolDictionary[key].Enqueue(obj);
        }
    }
}
