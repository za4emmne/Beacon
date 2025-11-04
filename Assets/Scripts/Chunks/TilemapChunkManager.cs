using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilemapChunkManager : MonoBehaviour
{
    public static TilemapChunkManager Instance { get; private set; }

    [Header("Chunk Prefab")]
    [SerializeField] private Grid gridPrefab;

    [Header("Biomes")]
    [SerializeField] private BiomeData[] biomes;
    [SerializeField] private float biomeScale = 0.05f; // Масштаб шума для биомов
    [SerializeField] private int seed = 12345; // Сид для воспроизводимости

    [Header("Chunk Settings")]
    [SerializeField] private int chunkSize = 16;
    [SerializeField] private int chunksVisible = 2;
    [SerializeField] private int poolSize = 15;

    [Header("Pooling Settings")]
    [SerializeField] private int prewarmDecorations = 20; // Прогрев пула декораций

    [Header("Noise Settings")]
    [SerializeField] private NoiseType noiseType = NoiseType.Perlin;
    [SerializeField] private float noiseScale = 0.1f;
    [SerializeField] private int octaves = 3; // Для фрактального шума
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

    private void Awake() => Instance = this;

    public void Init()
    {
        _player = Player.singleton.transform;
        random = new System.Random(seed);

        InitializeChunkPool();
        PrewarmDecorationPools();
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

    private void Update()
    {
        if (_player == null) return;

        Vector2Int playerChunk = WorldToChunk(_player.position);

        // Генерируем чанки вокруг игрока
        for (int x = -chunksVisible; x <= chunksVisible; x++)
        {
            for (int y = -chunksVisible; y <= chunksVisible; y++)
            {
                Vector2Int coord = playerChunk + new Vector2Int(x, y);
                if (!activeChunks.ContainsKey(coord))
                {
                    CreateChunk(coord);
                }
            }
        }

        // Удаляем далёкие чанки
        List<Vector2Int> toRemove = new();
        foreach (var coord in activeChunks.Keys)
        {
            if (Vector2Int.Distance(coord, playerChunk) > chunksVisible + 1)
            {
                toRemove.Add(coord);
            }
        }

        foreach (var coord in toRemove)
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

        // Определяем биом для этого чанка
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

        // Возвращаем все декорации в пул
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

        // Используем отдельный шум для биомов
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
        // Используем новый генератор шума
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

        // Используем детерминированный Random для воспроизводимости
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

    // Отладка биомов в Scene View
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
