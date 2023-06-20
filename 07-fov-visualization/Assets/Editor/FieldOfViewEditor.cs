using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        FieldOfView fieldOfView = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(
            fieldOfView.transform.position,
            Vector3.up,
            Vector3.forward,
            360,
            fieldOfView.viewRadius
        );

        Vector3 viewAngleA = fieldOfView.DirectionFromAngle(-fieldOfView.viewAngle / 2, false);
        Vector3 viewAngleB = fieldOfView.DirectionFromAngle(fieldOfView.viewAngle / 2, false);
        Handles.DrawLine(
            fieldOfView.transform.position,
            fieldOfView.transform.position + viewAngleA * fieldOfView.viewRadius
        );
        Handles.DrawLine(
            fieldOfView.transform.position,
            fieldOfView.transform.position + viewAngleB * fieldOfView.viewRadius
        );

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fieldOfView.visibleTargets)
        {
            Handles.DrawLine(fieldOfView.transform.position, visibleTarget.position);
        }
    }
}
