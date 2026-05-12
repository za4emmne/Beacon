using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TilemapChunkManager : MonoBehaviour
{
    public static TilemapChunkManager Instance { get; private set; }

    [Header("Chunk Prefab")]
    [SerializeField] private Grid gridPrefab;

    [Header("Biomes")]
    [SerializeField] private BiomeData[] biomes;
    [SerializeField] private float biomeScale = 0.05f;
    [SerializeField] private int seed = 12345;

    [Header("Chunk Settings")]
    [SerializeField] private int chunkSize = 16;
    [SerializeField] private int chunksVisible = 2;
    [SerializeField] private int poolSize = 15;

    [Header("Pooling Settings")]
    [SerializeField] private int prewarmDecorations = 20;

    [Header("Noise Settings")]
    [SerializeField] private NoiseType noiseType = NoiseType.Perlin;
    [SerializeField] private float noiseScale = 0.1f;
    [SerializeField] private int octaves = 3;
    [SerializeField] private float persistence = 0.5f;
    [SerializeField] private float lacunarity = 2f;

    private Dictionary<Vector2Int, ChunkInfo> activeChunks = new();
    private Queue<GameObject> chunkPool = new();
    private Transform _player;
    private System.Random random;

    [Header("Scripts")]
    [SerializeField] private SpawnerCoins _spawnerCoins;
    [SerializeField] private PillsGenerator _spawnerPills;

    private class ChunkInfo
    {
        public GameObject gameObject;
        public List<GameObject> spawnedObjects = new();
    }

    private void Awake()
    {
        InitDebug.Log($"[INIT][TilemapChunkManager] Awake() - Instance={Instance?.GetInstanceID()}, this={GetInstanceID()}, scene={SceneManager.GetActiveScene().name}");
        
        if (Instance != null && Instance != this)
        {
            InitDebug.LogWarning("[INIT][TilemapChunkManager] Duplicate! Destroying this");
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    private void OnDestroy()
    {
        InitDebug.Log($"[INIT][TilemapChunkManager] OnDestroy() - Instance={Instance?.GetInstanceID()}, this={GetInstanceID()}");
        
        if (Instance == this)
            Instance = null;
    }

    public void SetLocation(BiomeData location)
    {
        InitDebug.Log($"[INIT][TilemapChunkManager] SetLocation: {location?.name}");
        
                if (location != null)
        {
            biomes = new BiomeData[] { location };
        }
    }

    public void Init()
    {
        Init(null);
    }

    public void Init(BiomeData location)
    {
        InitDebug.Log("[INIT][TilemapChunkManager] Init() called");
        
        _player = Player.singleton?.transform;
        
        if (_player == null)
        {
            InitDebug.LogError("[INIT][TilemapChunkManager] Player.singleton is NULL!");
            return;
        }
        
        random = new System.Random(seed);

        InitializeChunkPool();
        PrewarmDecorationPools();
        
        GenerateInitialChunks();

        InitDebug.Log("[INIT][TilemapChunkManager] Init() complete - initial chunks generated");
    }
    
    private void GenerateInitialChunks()
    {
        Vector2Int playerChunk = WorldToChunk(_player.position);
        _lastPlayerChunk = playerChunk;
        
        InitDebug.Log($"[INIT][TilemapChunkManager] Generating initial chunks around player chunk {playerChunk}");
        
        for (int x = -chunksVisible; x <= chunksVisible; x++)
        {
            for (int y = -chunksVisible; y <= chunksVisible; y++)
            {
                Vector2Int coord = new Vector2Int(playerChunk.x + x, playerChunk.y + y);
                CreateChunk(coord);
            }
        }
        
        InitDebug.Log($"[INIT][TilemapChunkManager] Initial chunks generated around player");
    }

    private void InitializeChunkPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject chunk = Instantiate(gridPrefab.gameObject, transform);
            chunk.name = $"ChunkPooled_{i}";
            chunk.SetActive(false);
            chunkPool.Enqueue(chunk);
        }
    }

    private void PrewarmDecorationPools()
    {
        foreach (var biome in biomes)
        {
            foreach (var decoration in biome.decorations)
            {
                if (decoration.prefab != null)
                {
                    ObjectPool.Instance.Prewarm(decoration.prefab, prewarmDecorations);
                }
            }
        }
        Debug.Log("Decoration pools prewarmed");
    }

    private List<Vector2Int> _chunksToCreate = new();
    private List<Vector2Int> _chunksToRemove = new();
    private Vector2Int _lastPlayerChunk;

    private void Update()
    {
        if (_player == null) return;

        Vector2Int playerChunk = WorldToChunk(_player.position);

        // Только если игрок переместился в другой чанк
        if (playerChunk == _lastPlayerChunk) return;
        _lastPlayerChunk = playerChunk;

        // Очищаем списки переиспользованием
        _chunksToCreate.Clear();
        _chunksToRemove.Clear();

        // Создаём чанки вокруг игрока
        for (int x = -chunksVisible; x <= chunksVisible; x++)
        {
            for (int y = -chunksVisible; y <= chunksVisible; y++)
            {
                Vector2Int coord = new Vector2Int(playerChunk.x + x, playerChunk.y + y);
                if (!activeChunks.ContainsKey(coord))
                {
                    _chunksToCreate.Add(coord);
                }
            }
        }

        // Создаём новые чанки
        foreach (var coord in _chunksToCreate)
        {
            CreateChunk(coord);
        }

        // Удаляем дальние чанки
        foreach (var coord in activeChunks.Keys)
        {
            if (Vector2Int.Distance(coord, playerChunk) > chunksVisible + 1)
            {
                _chunksToRemove.Add(coord);
            }
        }

        foreach (var coord in _chunksToRemove)
        {
            ReturnChunkToPool(coord);
        }
    }

    private Vector2Int WorldToChunk(Vector2 pos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(pos.x / chunkSize),
            Mathf.FloorToInt(pos.y / chunkSize)
        );
    }

    private void CreateChunk(Vector2Int coord)
    {
        GameObject chunkObj = GetChunkFromPool();
        Vector3 worldPos = new Vector3(coord.x * chunkSize, coord.y * chunkSize, 0);
        chunkObj.transform.position = worldPos;
        chunkObj.name = $"Chunk_{coord.x}_{coord.y}";
        chunkObj.SetActive(true);

        Tilemap tilemap = chunkObj.GetComponentInChildren<Tilemap>();
        tilemap.ClearAllTiles();

        // ���������� ���� ��� ����� �����
        BiomeData biome = GetBiomeForChunk(coord);

        ChunkInfo chunkInfo = new ChunkInfo { gameObject = chunkObj };

        GenerateTiles(tilemap, coord, biome);
        SpawnDecorations(chunkObj.transform, coord, biome, chunkInfo);

        activeChunks.Add(coord, chunkInfo);
    }

    private GameObject GetChunkFromPool()
    {
        if (chunkPool.Count > 0)
        {
            return chunkPool.Dequeue();
        }
        else
        {
            Debug.LogWarning("Chunk pool exhausted, increase pool size");
            return Instantiate(gridPrefab.gameObject, transform);
        }
    }

    private void ReturnChunkToPool(Vector2Int coord)
    {
        ChunkInfo chunkInfo = activeChunks[coord];

        // ���������� ��� ��������� � ���
        foreach (var obj in chunkInfo.spawnedObjects)
        {
            ObjectPool.Instance.Release(obj);
        }
        chunkInfo.spawnedObjects.Clear();

        chunkInfo.gameObject.SetActive(false);
        chunkPool.Enqueue(chunkInfo.gameObject);
        activeChunks.Remove(coord);
    }

    private BiomeData GetBiomeForChunk(Vector2Int coord)
    {
        if (biomes.Length == 0) return null;

        // ���������� ��������� ��� ��� ������
        float noise = NoiseGenerator.Generate(
            coord.x,
            coord.y,
            noiseType,
            biomeScale,
            seed
        );

        int biomeIndex = Mathf.FloorToInt(noise * biomes.Length);
        biomeIndex = Mathf.Clamp(biomeIndex, 0, biomes.Length - 1);

        return biomes[biomeIndex];
    }

    private void GenerateTiles(Tilemap tilemap, Vector2Int chunkCoord, BiomeData biome)
    {
        if (biome == null || biome.tiles.Length == 0) return;

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                Vector3Int localPos = new Vector3Int(x, y, 0);
                int globalX = chunkCoord.x * chunkSize + x;
                int globalY = chunkCoord.y * chunkSize + y;

                TileBase selectedTile = SelectWeightedTile(globalX, globalY, biome.tiles);
                tilemap.SetTile(localPos, selectedTile);
            }
        }
    }

    private TileBase SelectWeightedTile(int x, int y, BiomeData.TileData[] tiles)
    {
        // ���������� ����� ��������� ����
        float noise = NoiseGenerator.GenerateFractal(
            x, y,
            noiseType,
            noiseScale,
            octaves,
            persistence,
            lacunarity,
            seed
        ) * 100f;

        float totalWeight = 0f;
        foreach (var tileData in tiles)
            totalWeight += tileData.weight;

        float randomValue = noise % totalWeight;
        float cumulative = 0f;

        foreach (var tileData in tiles)
        {
            cumulative += tileData.weight;
            if (randomValue <= cumulative)
                return tileData.tile;
        }

        return tiles[0].tile;
    }


    private void SpawnDecorations(Transform parent, Vector2Int chunkCoord, BiomeData biome, ChunkInfo chunkInfo)
    {
        if (biome == null || biome.decorations.Length == 0) return;

        // ���������� ����������������� Random ��� �����������������
        System.Random chunkRandom = new System.Random(seed + chunkCoord.x * 10000 + chunkCoord.y);

        for (int i = 0; i < biome.decorationsPerChunk; i++)
        {
            float randomX = (float)chunkRandom.NextDouble() * chunkSize;
            float randomY = (float)chunkRandom.NextDouble() * chunkSize;
            Vector3 worldPos = parent.position + new Vector3(randomX, randomY, 0);

            BiomeData.SpawnableObject selected = SelectSpawnableObject(biome.decorations, chunkRandom);
            if (selected != null && selected.prefab != null)
            {
                GameObject obj = ObjectPool.Instance.Get(selected.prefab, worldPos, parent);
                chunkInfo.spawnedObjects.Add(obj);

                if (obj.TryGetComponent<Loot>(out Loot loot))
                {
                    loot.Initialized(_spawnerCoins, _spawnerPills);

                }
            }
        }
    }

    private BiomeData.SpawnableObject SelectSpawnableObject(BiomeData.SpawnableObject[] objects, System.Random rng)
    {
        foreach (var obj in objects)
        {
            if (rng.NextDouble() * 100f <= obj.spawnChance)
                return obj;
        }
        return null;
    }

    // ������� ������ � Scene View
    void OnDrawGizmos()
    {
        if (!Application.isPlaying || biomes.Length == 0) return;

        foreach (var chunk in activeChunks)
        {
            BiomeData biome = GetBiomeForChunk(chunk.Key);
            if (biome != null)
            {
                Gizmos.color = biome.debugColor;
                Vector3 center = new Vector3(
                    chunk.Key.x * chunkSize + chunkSize * 0.5f,
                    chunk.Key.y * chunkSize + chunkSize * 0.5f,
                    0
                );
                Gizmos.DrawWireCube(center, new Vector3(chunkSize, chunkSize, 0));
            }
        }
    }
}
