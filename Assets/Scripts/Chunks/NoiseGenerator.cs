using UnityEngine;

public static class NoiseGenerator
{
    /// <summary>
    /// Генерирует шум по координатам
    /// </summary>
    public static float Generate(float x, float y, NoiseType type, float scale, int seed = 0)
    {
        x *= scale;
        y *= scale;

        switch (type)
        {
            case NoiseType.Perlin:
                return PerlinNoise(x, y);

            case NoiseType.Simplex:
                return SimplexNoise(x, y, seed);

            case NoiseType.Value:
                return ValueNoise(x, y, seed);

            case NoiseType.Voronoi:
                return VoronoiNoise(x, y, seed);

            case NoiseType.WhiteNoise:
                return WhiteNoise(x, y, seed);

            case NoiseType.Combined:
                return CombinedNoise(x, y, seed);

            default:
                return PerlinNoise(x, y);
        }
    }

    /// <summary>
    /// Фрактальный шум (несколько октав)
    /// </summary>
    public static float GenerateFractal(float x, float y, NoiseType type, float scale,
        int octaves, float persistence, float lacunarity, int seed = 0)
    {
        float total = 0f;
        float amplitude = 1f;
        float frequency = 1f;
        float maxValue = 0f;

        for (int i = 0; i < octaves; i++)
        {
            total += Generate(x * frequency, y * frequency, type, scale, seed + i) * amplitude;
            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= lacunarity;
        }

        return total / maxValue; // Нормализация
    }

    // ========== РЕАЛИЗАЦИИ РАЗНЫХ ТИПОВ ШУМА ==========

    static float PerlinNoise(float x, float y)
    {
        return Mathf.PerlinNoise(x, y);
    }

    static float SimplexNoise(float x, float y, int seed)
    {
        // Упрощённая версия Simplex (для полной используй Unity.Mathematics)
        float n0 = PerlinNoise(x + seed, y + seed);
        float n1 = PerlinNoise(x - seed * 0.5f, y + seed * 0.5f);
        return (n0 + n1) * 0.5f;
    }

    static float ValueNoise(float x, float y, int seed)
    {
        int xi = Mathf.FloorToInt(x);
        int yi = Mathf.FloorToInt(y);

        float tx = x - xi;
        float ty = y - yi;

        // Сглаживание (Smoothstep)
        tx = tx * tx * (3f - 2f * tx);
        ty = ty * ty * (3f - 2f * ty);

        float v00 = Hash2D(xi, yi, seed);
        float v10 = Hash2D(xi + 1, yi, seed);
        float v01 = Hash2D(xi, yi + 1, seed);
        float v11 = Hash2D(xi + 1, yi + 1, seed);

        float v0 = Mathf.Lerp(v00, v10, tx);
        float v1 = Mathf.Lerp(v01, v11, tx);

        return Mathf.Lerp(v0, v1, ty);
    }

    static float VoronoiNoise(float x, float y, int seed)
    {
        int xi = Mathf.FloorToInt(x);
        int yi = Mathf.FloorToInt(y);

        float minDist = float.MaxValue;

        // Проверяем соседние ячейки
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int cellX = xi + dx;
                int cellY = yi + dy;

                // Случайная точка внутри ячейки
                float pointX = cellX + Hash2D(cellX, cellY, seed);
                float pointY = cellY + Hash2D(cellY, cellX, seed + 1);

                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(pointX, pointY));
                minDist = Mathf.Min(minDist, dist);
            }
        }

        return Mathf.Clamp01(minDist);
    }

    static float WhiteNoise(float x, float y, int seed)
    {
        return Hash2D(Mathf.FloorToInt(x), Mathf.FloorToInt(y), seed);
    }

    static float CombinedNoise(float x, float y, int seed)
    {
        // Комбинация Perlin + Voronoi
        float perlin = PerlinNoise(x, y);
        float voronoi = VoronoiNoise(x * 2f, y * 2f, seed);
        return perlin * 0.7f + voronoi * 0.3f;
    }

    // ========== ВСПОМОГАТЕЛЬНЫЕ ФУНКЦИИ ==========

    static float Hash2D(int x, int y, int seed)
    {
        int hash = x * 374761393 + y * 668265263 + seed;
        hash = (hash ^ (hash >> 13)) * 1274126177;
        return ((hash & 0x7FFFFFFF) / (float)0x7FFFFFFF);
    }
}

public enum NoiseType
{
    Perlin,
    Simplex,
    Value,
    Voronoi,
    WhiteNoise,
    Combined
}
