using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeCreator))]
public class ShapeEditor : Editor
{
    ShapeCreator shapeCreator;
    SelectionInfo selectionInfo;
    bool needsRepaint;

    private void OnSceneGUI()
    {
        Event guiEvent = Event.current;

        if (guiEvent.type == EventType.Repaint)
        {
            Draw();
        }
        // note: left-clicking on the scene will deselect the main game
        // object this forces the user to reselect the object to add a
        // second point this condition helps us get around this
        else if (guiEvent.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
        else
        {
            HandleInput(guiEvent);
            if (needsRepaint)
            {
                HandleUtility.Repaint();
            }
        }
    }

    void HandleInput(Event guiEvent)
    {
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);

        float drawPlaneHeight = 0;
        float distToDrawPlane = (drawPlaneHeight - mouseRay.origin.y) / mouseRay.direction.y;
        // Vector3 mousePosition = mouseRay.origin + mouseRay.direction * distToDrawPlane;
        Vector3 mousePosition = mouseRay.GetPoint(distToDrawPlane);

        // note: unsure if EventModifiers.None check is required
        if (
            guiEvent.type == EventType.MouseDown
            && guiEvent.button == 0
            && guiEvent.modifiers == EventModifiers.None
        )
        {
            HandleLeftMouseDown(mousePosition);
        }

        if (
            guiEvent.type == EventType.MouseUp
            && guiEvent.button == 0
            && guiEvent.modifiers == EventModifiers.None
        )
        {
            HandleLeftMouseUp(mousePosition);
        }
        if (
            guiEvent.type == EventType.MouseDrag
            && guiEvent.button == 0
            && guiEvent.modifiers == EventModifiers.None
        )
        {
            HandleLeftMouseDrag(mousePosition);
        }

        if (!selectionInfo.pointIsSelected)
        {
            UpdateMouseOverInfo(mousePosition);
        }
    }

    void HandleLeftMouseDown(Vector3 mousePosition)
    {
        if (!selectionInfo.mouseIsOverPoint)
        {
            int newPointIndex =
                (selectionInfo.mouseIsOverLine)
                    ? selectionInfo.lineIndex + 1
                    : shapeCreator.points.Count;
            Undo.RecordObject(shapeCreator, "Add point");
            shapeCreator.points.Insert(newPointIndex, mousePosition);
            selectionInfo.pointIndex = newPointIndex;
        }

        selectionInfo.pointIsSelected = true;
        selectionInfo.positionAtStartOfDrag = mousePosition;
        needsRepaint = true;
    }

    void HandleLeftMouseUp(Vector3 mousePosition)
    {
        if (selectionInfo.pointIsSelected)
        {
            shapeCreator.points[selectionInfo.pointIndex] = selectionInfo.positionAtStartOfDrag;
            Undo.RecordObject(shapeCreator, "Move point");
            shapeCreator.points[selectionInfo.pointIndex] = mousePosition;

            selectionInfo.pointIsSelected = false;
            selectionInfo.pointIndex = -1;
            needsRepaint = true;
        }
    }

    void HandleLeftMouseDrag(Vector3 mousePosition)
    {
        if (selectionInfo.pointIsSelected)
        {
            shapeCreator.points[selectionInfo.pointIndex] = mousePosition;
            needsRepaint = true;
        }
    }

    void UpdateMouseOverInfo(Vector3 mousePosition)
    {
        int mouseOverPointIndex = -1;
        for (int i = 0; i < shapeCreator.points.Count; i++)
        {
            if (Vector3.Distance(mousePosition, shapeCreator.points[i]) < shapeCreator.handleRadius)
            {
                mouseOverPointIndex = i;
                break;
            }
        }

        if (mouseOverPointIndex != selectionInfo.pointIndex)
        {
            selectionInfo.pointIndex = mouseOverPointIndex;
            selectionInfo.mouseIsOverPoint = mouseOverPointIndex != -1;

            needsRepaint = true;
        }

        if (selectionInfo.mouseIsOverPoint)
        {
            selectionInfo.mouseIsOverLine = false;
            selectionInfo.lineIndex = -1;
        }
        else
        {
            int mouseOverLineIndex = -1;
            float closestLineDist = shapeCreator.handleRadius;
            for (int i = 0; i < shapeCreator.points.Count; i++)
            {
                Vector3 currentPoint = shapeCreator.points[i];
                Vector3 nextPointInShape = shapeCreator.points[(i + 1) % shapeCreator.points.Count];
                float distFromMouseToLine = HandleUtility.DistancePointToLineSegment(
                    mousePosition.ToXZ(),
                    currentPoint.ToXZ(),
                    nextPointInShape.ToXZ()
                );
                if (distFromMouseToLine < closestLineDist)
                {
                    closestLineDist = distFromMouseToLine;
                    mouseOverLineIndex = i;
                }
            }

            if (selectionInfo.lineIndex != mouseOverLineIndex)
            {
                selectionInfo.lineIndex = mouseOverLineIndex;
                selectionInfo.mouseIsOverLine = mouseOverLineIndex != -1;
                needsRepaint = true;
            }
        }
    }

    void Draw()
    {
        for (int i = 0; i < shapeCreator.points.Count; i++)
        {
            Vector3 currPoint = shapeCreator.points[i];
            Vector3 nextPoint = shapeCreator.points[(i + 1) % shapeCreator.points.Count];

            if (i == selectionInfo.lineIndex)
            {
                Handles.color = Color.red;
                Handles.DrawLine(currPoint, nextPoint, 4);
            }
            else
            {
                Handles.color = Color.black;
                Handles.DrawDottedLine(currPoint, nextPoint, 4);
            }

            if (i == selectionInfo.pointIndex)
            {
                Handles.color = (selectionInfo.pointIsSelected) ? Color.black : Color.red;
            }
            else
            {
                Handles.color = Color.white;
            }
            Handles.DrawSolidDisc(currPoint, Vector3.up, shapeCreator.handleRadius);
        }

        needsRepaint = false;
    }

    void OnEnable()
    {
        shapeCreator = target as ShapeCreator;
        selectionInfo = new SelectionInfo();
    }

    public class SelectionInfo
    {
        public int pointIndex = -1;
        public bool mouseIsOverPoint;
        public bool pointIsSelected;
        public Vector3 positionAtStartOfDrag;

        public int lineIndex = -1;
        public bool mouseIsOverLine;
    }
}
