using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewBiome", menuName = "Game/Biome Data")]
public class BiomeData : ScriptableObject
{
    [Header("Biome Info")]
    public string biomeName;
    public Color debugColor = Color.white; // Для отладки в Scene View

    [Header("Tiles")]
    public TileData[] tiles;

    [Header("Decorations")]
    public SpawnableObject[] decorations;
    [Range(0, 20)] public int decorationsPerChunk = 5;

    [System.Serializable]
    public class TileData
    {
        public TileBase tile;
        [Range(0f, 100f)] public float weight = 50f;
    }

    [System.Serializable]
    public class SpawnableObject
    {
        public GameObject prefab;
        [Range(0f, 100f)] public float spawnChance = 30f;
    }
}
