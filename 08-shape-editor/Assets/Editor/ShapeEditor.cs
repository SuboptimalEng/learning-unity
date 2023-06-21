using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShapeCreator))]
public class ShapeEditor : Editor
{
    ShapeCreator shapeCreator;
    bool needsRepaint;

    private void OnSceneGUI()
    {
        Event guiEvent = Event.current;

        if (guiEvent.type == EventType.Repaint)
        {
            Draw();
        }
        // left-clicking on the scene will deselect the main game object
        // this forces the user to reselect the object to add a second point
        // this condition helps us get around this
        else if (guiEvent.type == EventType.Layout)
        {
            // commenting this line out still works, I wonder if this is not needed
            // in the newer versions of Unity
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

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
        {
            Undo.RecordObject(shapeCreator, "Add point");
            shapeCreator.points.Add(mousePosition);
            Debug.Log("add: " + mousePosition);
            needsRepaint = true;
        }
    }

    void Draw()
    {
        for (int i = 0; i < shapeCreator.points.Count; i++)
        {
            Vector3 currPoint = shapeCreator.points[i];
            Vector3 nextPoint = shapeCreator.points[(i + 1) % shapeCreator.points.Count];

            Handles.color = Color.black;
            Handles.DrawDottedLine(currPoint, nextPoint, 4);
            Handles.color = Color.white;
            Handles.DrawSolidDisc(currPoint, Vector3.up, 0.5f);
        }

        needsRepaint = false;
    }

    void OnEnable()
    {
        shapeCreator = target as ShapeCreator;
    }
}
