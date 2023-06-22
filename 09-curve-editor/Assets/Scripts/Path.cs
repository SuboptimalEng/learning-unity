using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path
{
    [SerializeField, HideInInspector]
    List<Vector2> points;

    [SerializeField, HideInInspector]
    bool isClosed;

    public Path(Vector2 center)
    {
        points = new List<Vector2>
        {
            center + Vector2.left,
            center + (Vector2.left + Vector2.up) * 0.5f,
            center + (Vector2.right + Vector2.down) * 0.5f,
            center + Vector2.right,
        };
    }

    public Vector2 this[int i]
    {
        get { return points[i]; }
    }

    public int NumPoints
    {
        get { return points.Count; }
    }

    public int NumSegments
    {
        get { return points.Count / 3; }
    }

    public void AddSegment(Vector2 anchorPos)
    {
        points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
        points.Add((points[points.Count - 1] + anchorPos) * 0.5f);
        points.Add(anchorPos);
    }

    public Vector2[] GetPointsInSegment(int i)
    {
        return new Vector2[]
        {
            points[i * 3],
            points[i * 3 + 1],
            points[i * 3 + 2],
            points[LoopIndex(i * 3 + 3)]
        };
    }

    public void MovePoint(int i, Vector2 pos)
    {
        Vector2 deltaMove = pos - points[i];
        points[i] = pos;

        // moving anchor point if i % 3 == 0
        // otherwise moving control point
        if (i % 3 == 0)
        {
            if (i + 1 < points.Count || isClosed)
            {
                points[LoopIndex(i + 1)] += deltaMove;
            }
            if (i - 1 >= 0 || isClosed)
            {
                points[LoopIndex(i - 1)] += deltaMove;
            }
        }
        else
        {
            bool nextPointIsAnchor = (i + 1) % 3 == 0;
            int correspondingControlIndex = (nextPointIsAnchor) ? i + 2 : i - 2;
            int anchorIndex = (nextPointIsAnchor) ? i + 1 : i - 1;

            if (
                correspondingControlIndex >= 0 && correspondingControlIndex < points.Count
                || isClosed
            )
            {
                float dist = (
                    points[LoopIndex(anchorIndex)] - points[LoopIndex(correspondingControlIndex)]
                ).magnitude;
                Vector2 dir = (points[LoopIndex(anchorIndex)] - pos).normalized;
                points[LoopIndex(correspondingControlIndex)] =
                    points[LoopIndex(anchorIndex)] + dir * dist;
            }
        }
    }

    public void ToggleClosed()
    {
        isClosed = !isClosed;

        // add two control points which closes the loop
        if (isClosed)
        {
            points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
            points.Add(points[0] * 2 - points[1]);
        }
        else
        {
            points.RemoveRange(points.Count - 2, 2);
        }
    }

    int LoopIndex(int i)
    {
        return (i + points.Count) % points.Count;
    }
}
