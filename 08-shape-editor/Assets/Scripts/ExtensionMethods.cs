using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    // public static Vector2 ToXZ(Vector3 v3)
    // passing in 'this' allows us to call this method directly on a Vector3
    // e.g. mousePosition.ToXZ(), currentPoint.ToXZ(), nextPointInShape.ToXZ()
    public static Vector2 ToXZ(this Vector3 v3)
    {
        return new Vector2(v3.x, v3.z);
    }
}
