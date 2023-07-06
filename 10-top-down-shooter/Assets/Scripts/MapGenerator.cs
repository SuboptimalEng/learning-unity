using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Map[] maps;
    public int mapIndex;

    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Transform navmeshFloor;
    public Transform navmeshMaskPrefab;
    public Vector2 maxMapSize;

    [Range(0, 1)]
    public float outlinePercent;

    public float tileSize;
    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;

    Map currentMap;

    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        currentMap = maps[mapIndex];
        System.Random prng = new System.Random(currentMap.seed);

        // generating coords
        allTileCoords = new List<Coord>();
        for (int x = 0; x < currentMap.mapSize.x; x++)
        {
            for (int y = 0; y < currentMap.mapSize.y; y++)
            {
                allTileCoords.Add(new Coord(x, y));
            }
        }
        shuffledTileCoords = new Queue<Coord>(
            Utility.ShuffleArray(allTileCoords.ToArray(), currentMap.seed)
        );

        // create map holder object
        string holderName = "Generated Map";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        // spawning tiles
        for (int x = 0; x < currentMap.mapSize.x; x++)
        {
            for (int y = 0; y < currentMap.mapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(
                    tilePrefab,
                    tilePosition,
                    Quaternion.Euler(Vector3.right * 90)
                );
                newTile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
                newTile.parent = mapHolder;
            }
        }

        // spawning obstacles
        bool[,] obstacleMap = new bool[(int)currentMap.mapSize.x, (int)currentMap.mapSize.y];

        int obstacleCount = (int)(
            currentMap.mapSize.x * currentMap.mapSize.y * currentMap.obstaclePercent
        );
        int currentObstacleCount = 0;

        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount++;

            if (
                randomCoord != currentMap.mapCenter
                && MapIsFullyAccessible(obstacleMap, currentObstacleCount)
            )
            {
                float obstacleHeight = Mathf.Lerp(
                    currentMap.minObstacleHeight,
                    currentMap.maxObstacleHeight,
                    (float)prng.NextDouble()
                );
                Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);
                Transform newObstacle =
                    Instantiate(
                        obstaclePrefab,
                        obstaclePosition + Vector3.up * obstacleHeight / 2,
                        Quaternion.identity
                    ) as Transform;
                newObstacle.parent = mapHolder;
                newObstacle.localScale = new Vector3(
                    (1 - outlinePercent) * tileSize,
                    obstacleHeight,
                    (1 - outlinePercent) * tileSize
                );
            }
            else
            {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount--;
            }
        }

        // creating the navmesh mask
        Transform maskLeft =
            Instantiate(
                navmeshMaskPrefab,
                Vector3.left * (currentMap.mapSize.x + maxMapSize.x) / 4 * tileSize,
                Quaternion.identity
            ) as Transform;
        maskLeft.parent = mapHolder;
        maskLeft.localScale =
            new Vector3((maxMapSize.x - currentMap.mapSize.x) / 2, 1, currentMap.mapSize.y)
            * tileSize;

        Transform maskRight =
            Instantiate(
                navmeshMaskPrefab,
                Vector3.right * (currentMap.mapSize.x + maxMapSize.x) / 4 * tileSize,
                Quaternion.identity
            ) as Transform;
        maskRight.parent = mapHolder;
        maskRight.localScale =
            new Vector3((maxMapSize.x - currentMap.mapSize.x) / 2, 1, currentMap.mapSize.y)
            * tileSize;

        Transform maskTop =
            Instantiate(
                navmeshMaskPrefab,
                Vector3.forward * (currentMap.mapSize.y + maxMapSize.y) / 4 * tileSize,
                Quaternion.identity
            ) as Transform;
        maskTop.parent = mapHolder;
        maskTop.localScale =
            new Vector3(maxMapSize.x, 1, (maxMapSize.y - currentMap.mapSize.y) / 2) * tileSize;

        Transform maskBottom =
            Instantiate(
                navmeshMaskPrefab,
                Vector3.back * (currentMap.mapSize.y + maxMapSize.y) / 4 * tileSize,
                Quaternion.identity
            ) as Transform;
        maskBottom.parent = mapHolder;
        maskBottom.localScale =
            new Vector3(maxMapSize.x, 1, (maxMapSize.y - currentMap.mapSize.y) / 2) * tileSize;

        navmeshFloor.localScale = new Vector3(maxMapSize.x, maxMapSize.y) * tileSize;
    }

    bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(currentMap.mapCenter);
        mapFlags[currentMap.mapCenter.x, currentMap.mapCenter.y] = true;

        int accessibleTileCount = 1;

        while (true)
        {
            if (queue.Count == 0)
            {
                break;
            }

            Coord tile = queue.Dequeue();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighborX = tile.x + x;
                    int neighborY = tile.y + y;

                    if (x == 0 || y == 0)
                    {
                        if (
                            neighborX >= 0
                            && neighborX < obstacleMap.GetLength(0)
                            && neighborY >= 0
                            && neighborY < obstacleMap.GetLength(1)
                        )
                        {
                            if (
                                !mapFlags[neighborX, neighborY]
                                && !obstacleMap[neighborX, neighborY]
                            )
                            {
                                mapFlags[neighborX, neighborY] = true;
                                queue.Enqueue(new Coord(neighborX, neighborY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(
            currentMap.mapSize.x * currentMap.mapSize.y - currentObstacleCount
        );
        return targetAccessibleTileCount == accessibleTileCount;
    }

    Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(
                -currentMap.mapSize.x / 2 + 0.5f + x,
                0,
                -currentMap.mapSize.y / 2 + 0.5f + y
            ) * tileSize;
    }

    public Coord GetRandomCoord()
    {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    [System.Serializable]
    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Coord a, Coord b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Coord a, Coord b)
        {
            return !(a == b);
        }
    }

    [System.Serializable]
    public class Map
    {
        public Coord mapSize;

        [Range(0, 1)]
        public float obstaclePercent;
        public int seed;
        public float minObstacleHeight;
        public float maxObstacleHeight;
        public Color foreground;
        public Color background;

        public Coord mapCenter
        {
            get { return new Coord(mapSize.x / 2, mapSize.y / 2); }
        }
    }
}
