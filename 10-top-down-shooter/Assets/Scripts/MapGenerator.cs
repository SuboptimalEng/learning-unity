using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab;
    public Vector2 mapSize;

    [Range(0, 1)]
    public float outlinePercent;

    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        string holderName = "Generated Map";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(
                    -mapSize.x / 2 + 0.5f + x,
                    0,
                    -mapSize.y / 2 + 0.5f + y
                );
                Transform newTile = Instantiate(
                    tilePrefab,
                    tilePosition,
                    Quaternion.Euler(Vector3.right * 90)
                );
                newTile.localScale = Vector3.one * (1 - outlinePercent);
                newTile.parent = mapHolder;
            }
        }
    }
}
