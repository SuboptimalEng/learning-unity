using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator {
  public static void GenerateTerrainMesh(float[,] heightMap) {
    int width = heightMap.GetLength(0);
    int height = heightMap.GetLength(1);

    MeshData meshData = new MeshData(width, height);
    int vertexIndex = 0;

    for (int y = 0; y < height; y++) {



      for (int x = 0; x < width; x++) {


        meshData.vertices[vertexIndex] = new Vector3(x, heightMap[x, y], y);
      }
    }
  }
}

public class MeshData {
  public Vector3[] vertices;
  public int[] triangles;

  int triangleIndex;

  public MeshData(int meshWidth, int meshHeight) {
    vertices = new Vector3[meshWidth * meshHeight];
    triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
  }

  public void AddTriangle(int a, int b, int c) {
    triangles[triangleIndex] = a;
    triangles[triangleIndex + 1] = b;
    triangles[triangleIndex + 2] = c;
    triangleIndex += 3;
  }
}
