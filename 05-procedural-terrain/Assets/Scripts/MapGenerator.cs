using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        NoiseMap,
        ColorMap,
        Mesh
    };

    public DrawMode drawMode;

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

    public TerrainType[] regions;

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

        Color[] colorMap = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight < regions[i].height)
                    {
                        // note: why is this y * mapWidth?
                        colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindAnyObjectByType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            display.DrawTexture(
                TextureGenerator.TextureFromColorMap(colorMap, mapHeight, mapHeight)
            );
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(
                MeshGenerator.GenerateTerrainMesh(noiseMap),
                TextureGenerator.TextureFromColorMap(colorMap, mapHeight, mapHeight)
            );
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}
