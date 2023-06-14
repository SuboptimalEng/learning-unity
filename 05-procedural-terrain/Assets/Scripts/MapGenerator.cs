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

    public const int mapChunkSize = 241;

    [Range(0, 6)]
    public int levelOfDetail;

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

    [Range(1, 200)]
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public Vector2 offset;

    public bool autoUpdate;

    public TerrainType[] regions;

    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData();

        MapDisplay display = FindAnyObjectByType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            display.DrawTexture(
                TextureGenerator.TextureFromColorMap(mapData.colorMap, mapChunkSize, mapChunkSize)
            );
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(
                MeshGenerator.GenerateTerrainMesh(
                    mapData.heightMap,
                    meshHeightMultiplier,
                    meshHeightCurve,
                    levelOfDetail
                ),
                TextureGenerator.TextureFromColorMap(mapData.colorMap, mapChunkSize, mapChunkSize)
            );
        }
    }

    MapData GenerateMapData()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(
            mapChunkSize,
            mapChunkSize,
            seed,
            noiseScale,
            octaves,
            persistance,
            lacunarity,
            offset
        );

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight < regions[i].height)
                    {
                        // note: why is this y * mapWidth?
                        colorMap[y * mapChunkSize + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        return new MapData(noiseMap, colorMap);
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}

public struct MapData
{
    public float[,] heightMap;
    public Color[] colorMap;

    public MapData(float[,] heightMap, Color[] colorMap)
    {
        this.heightMap = heightMap;
        this.colorMap = colorMap;
    }
}
