using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Range(1, 100)]
    public int mapWidth;

    [Range(1, 100)]
    public int mapHeight;

    [Range(1, 50)]
    public float noiseScale;

    [Range(1, 8)]
    public int octaves;

    [Range(0, 1)]
    public float persistance;

    [Range(1, 4)]
    public float lacunarity;

    [Range(1, 100)]
    public int seed;

    public Vector2 offset;

    public bool autoUpdate;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(
            mapWidth,
            mapHeight,
            seed,
            noiseScale,
            octaves,
            persistance,
            lacunarity,
            offset
        );

        MapDisplay display = FindAnyObjectByType<MapDisplay>();
        display.DrawNoiseMap(noiseMap);
    }
}
