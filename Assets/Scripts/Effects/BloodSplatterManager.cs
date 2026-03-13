using System.Collections.Generic;
using UnityEngine;

public class BloodSplatterManager : MonoBehaviour
{
    public static BloodSplatterManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private int _maxSplatters = 50;
    [SerializeField] private float _splatterLifetime = 15f;
    [SerializeField] private float _fadeStartTime = 12f;
    [SerializeField] private float _splatterSizeMin = 0.3f;
    [SerializeField] private float _splatterSizeMax = 0.8f;
    [SerializeField] private int _poolSize = 60;

    [Header("References")]
    [SerializeField] private GameObject _splatterPrefab;
    [SerializeField] private Sprite[] _bloodSprites;

    private Queue<BloodDecal> _pool = new Queue<BloodDecal>();
    private List<BloodDecal> _activeSplatters = new List<BloodDecal>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializePool();
        CreateDefaultSprites();
    }

    private void InitializePool()
    {
        if (_splatterPrefab == null)
        {
            _splatterPrefab = CreateSplatterPrefab();
        }

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obj = Instantiate(_splatterPrefab, transform);
            BloodDecal decal = obj.GetComponent<BloodDecal>();
            decal.OnDecalExpired += () => ReturnToPool(decal);
            obj.SetActive(false);
            _pool.Enqueue(decal);
        }
    }

    private GameObject CreateSplatterPrefab()
    {
        GameObject prefab = new GameObject("BloodSplatterPrefab");
        
        SpriteRenderer sr = prefab.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 100;
        sr.color = Color.red;
        
        BloodDecal decal = prefab.AddComponent<BloodDecal>();
        
        prefab.SetActive(false);
        return prefab;
    }

    private void CreateDefaultSprites()
    {
        if (_bloodSprites == null || _bloodSprites.Length == 0)
        {
            _bloodSprites = new Sprite[8];
            
            for (int i = 0; i < 8; i++)
            {
                Texture2D tex = new Texture2D(64, 64);
                Color[] colors = new Color[64 * 64];
                
                float centerX = Random.Range(22f, 42f);
                float centerY = Random.Range(22f, 42f);
                float baseRadius = Random.Range(18f, 28f);
                
                for (int y = 0; y < 64; y++)
                {
                    for (int x = 0; x < 64; x++)
                    {
                        float dist = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
                        float noise1 = Mathf.PerlinNoise(x * 0.1f + i * 7f, y * 0.1f) * 12f;
                        float noise2 = Mathf.PerlinNoise(x * 0.25f + i * 3f, y * 0.25f + 50f) * 6f;
                        float noise3 = Mathf.PerlinNoise(x * 0.5f + i * 5f, y * 0.5f + 100f) * 3f;
                        
                        float noise = noise1 + noise2 + noise3;
                        float threshold = baseRadius + noise;
                        
                        float squareDist = Mathf.Max(Mathf.Abs(x - centerX), Mathf.Abs(y - centerY));
                        
                        if (squareDist < threshold)
                        {
                            float edgeDist = threshold - squareDist;
                            float alpha = Mathf.Clamp01(edgeDist / 8f);
                            alpha *= Random.Range(0.75f, 1f);
                            
                            if (Random.value > 0.85f)
                                alpha *= Random.Range(0.3f, 0.7f);
                            
                            colors[y * 64 + x] = new Color(0.65f + Random.Range(-0.05f, 0.05f), 0.08f, 0.08f, alpha);
                        }
                        else
                        {
                            colors[y * 64 + x] = Color.clear;
                        }
                    }
                }
                
                tex.SetPixels(colors);
                tex.Apply();
                
                _bloodSprites[i] = Sprite.Create(tex, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect);
            }
        }
    }

    public void SpawnSplatter(Vector3 position, Vector3 direction)
    {
        if (_activeSplatters.Count >= _maxSplatters)
        {
            BloodDecal oldest = _activeSplatters[0];
            if (oldest != null)
            {
                _activeSplatters.RemoveAt(0);
                if (oldest.gameObject != null)
                    Destroy(oldest.gameObject);
            }
        }

        BloodDecal decal = GetFromPool();
        if (decal == null) return;

        decal.transform.position = new Vector3(position.x, position.y, 0);
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        decal.SetRotation(angle + Random.Range(-20f, 20f));
        
        float size = Random.Range(_splatterSizeMin, _splatterSizeMax);
        decal.SetSize(size);
        
        if (_bloodSprites != null && _bloodSprites.Length > 0)
            decal.SetSprite(_bloodSprites[Random.Range(0, _bloodSprites.Length)]);
        
        decal.Initialize(_splatterLifetime, _fadeStartTime);
        
        _activeSplatters.Add(decal);
    }

    public void SpawnSplatterAtDeath(Vector3 position)
    {
        if (_activeSplatters.Count >= _maxSplatters)
        {
            BloodDecal oldest = _activeSplatters[0];
            if (oldest != null)
            {
                _activeSplatters.RemoveAt(0);
                if (oldest.gameObject != null)
                    Destroy(oldest.gameObject);
            }
        }

        BloodDecal decal = GetFromPool();
        if (decal == null) return;

        decal.transform.position = new Vector3(position.x, position.y, 0);
        
        decal.SetRotation(Random.Range(0f, 360f));
        
        float size = Random.Range(_splatterSizeMin * 1.2f, _splatterSizeMax * 1.5f);
        decal.SetSize(size);
        
        if (_bloodSprites != null && _bloodSprites.Length > 0)
            decal.SetSprite(_bloodSprites[Random.Range(0, _bloodSprites.Length)]);
        
        Color colorVariation = new Color(
            Random.Range(0.5f, 0.8f),
            Random.Range(0.05f, 0.15f),
            Random.Range(0.05f, 0.15f),
            1f
        );
        decal.SetColor(colorVariation);
        
        decal.Initialize(_splatterLifetime, _fadeStartTime);
        
        _activeSplatters.Add(decal);
    }

    private BloodDecal GetFromPool()
    {
        BloodDecal decal;
        
        if (_pool.Count > 0)
        {
            decal = _pool.Dequeue();
        }
        else
        {
            GameObject obj = Instantiate(_splatterPrefab, transform);
            decal = obj.GetComponent<BloodDecal>();
            decal.OnDecalExpired += () => ReturnToPool(decal);
        }
        
        if (decal != null)
        {
            decal.gameObject.SetActive(true);
        }
        
        return decal;
    }

    private void ReturnToPool(BloodDecal decal)
    {
        if (decal == null) return;
        
        _activeSplatters.Remove(decal);
        
        if (decal.gameObject != null)
        {
            decal.gameObject.SetActive(false);
            _pool.Enqueue(decal);
        }
    }

    public void ClearAllSplatters()
    {
        foreach (var splatter in _activeSplatters)
        {
            if (splatter != null && splatter.gameObject != null)
            {
                splatter.gameObject.SetActive(false);
                _pool.Enqueue(splatter);
            }
        }
        _activeSplatters.Clear();
    }

    public int ActiveCount => _activeSplatters.Count;
    public int PoolCount => _pool.Count;
}
