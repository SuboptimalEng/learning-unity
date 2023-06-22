using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeCreator : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    [HideInInspector]
    public List<Shape> shapes = new List<Shape>();

    [HideInInspector]
    public bool showShapesList;

    public float handleRadius = 0.5f;

    public void UpdateMeshDisplay() { }
}

[System.Serializable]
public class Shape
{
    public List<Vector3> points = new List<Vector3>();
}
