using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sebastian.Geometry;

public class ShapeCreator : MonoBehaviour
{
    public MeshFilter meshFilter;

    [HideInInspector]
    public List<Shape> shapes = new List<Shape>();

    [HideInInspector]
    public bool showShapesList;

    public float handleRadius = 0.5f;

    public void UpdateMeshDisplay()
    {
        CompositeShape compShape = new CompositeShape(shapes);
        meshFilter.mesh = compShape.GetMesh();
    }
}

// note: use shape class from from Sebastian's geometry class
// [System.Serializable]
// public class Shape
// {
//     public List<Vector3> points = new List<Vector3>();
// }
