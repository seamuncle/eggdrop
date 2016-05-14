using System;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using UnityEditor;
public class TrailMenu : EditorWindow
{

    [MenuItem("Trail/Insert Loop Points %e")]

    static void InsertLoopPoint()
    {
        GameObject[] selected = Selection.gameObjects;

        Trail trail = null;
        List<TrailPoint> selectedPoints = new List<TrailPoint>();
        foreach (GameObject go in selected)
        {
            if(trail == null )
            {
                trail = go.GetComponent<Trail>();

            }
            TrailPoint point = go.GetComponent<TrailPoint>();
            if (point == null)
            {
                continue;
            }
            selectedPoints.Add(point);

            if (trail == null)
            {
                trail = point.transform.parent.GetComponent<Trail>();
            }

        }

        int lastPointIndex = trail.points.Count - 1;
        if (selectedPoints.Count == 0)
        {
            GameObject go = new GameObject("***");
            go.transform.position = trail.transform.position;
            go.transform.parent = trail.transform;
            trail.points.Add(go.AddComponent<TrailPoint>());

        }
        else
        {
            for (int i = lastPointIndex; i >= 0; i--)
            {
                if (selectedPoints.Count == 1 && selectedPoints.Contains(trail.points[i]))
                {
                    GameObject go = new GameObject("***");
                    go.transform.position = Vector3.Lerp(trail.GetClampedPointFor(i), trail.GetClampedPointFor(i + 1), 0.5f);
                    go.transform.parent = trail.transform;
                    trail.points.Insert(i + 1, go.AddComponent<TrailPoint>());
                    break;

                }
                else if (selectedPoints.Count > 1 && selectedPoints.Contains(trail.points[i - 1]))
                {

                    GameObject go = new GameObject("***");
                    go.transform.position = Vector3.Lerp(trail.GetClampedPointFor(i - 1), trail.GetClampedPointFor(i), 0.5f);
                    go.transform.parent = trail.transform;
                    trail.points.Insert(i, go.AddComponent<TrailPoint>());
                }

            }
        }
        trail.CleanChildren();
    }
}