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
        GameObject parent;
        Trail trail;
        if (selected.Length == 0)
        {
            Type[] components = new Type[] { typeof(Trail) };
            parent = new GameObject("Trail", components);
        } else
        {
            parent = selected[0].transform.parent.gameObject;
        }
        trail = parent.GetComponent<Trail>();

        List<TrailPoint> selectedPoints = new List<TrailPoint>();
        foreach ( GameObject point in selected )
        {
            selectedPoints.Add(point.GetComponent<TrailPoint>());
        }
        
        for(int i = trail.points.Count-1; i > 0; i--)
        {
            if (  selectedPoints.Contains( trail.points[i] ) && selectedPoints.Contains(trail.points[i-1]) )
            {
                Type[] components = new Type[] { typeof(TrailPoint) };

                GameObject go = new GameObject("***",components);
                go.transform.position = Vector3.Lerp(trail.points[i].position, trail.points[i - 1].position, 0.5f);
                go.transform.parent = trail.transform;
                trail.points.Insert(i, go.GetComponent<TrailPoint>());
            }

        }
        for (int i = 0; i < trail.points.Count; i++)
        {
            trail.points[i].name = "Point" + (i + 1);
            trail.points[i].transform.SetSiblingIndex(i);
        }



        //        EditorWindow window = GetWindow<TrailMenu>();
        //        window.Show();
    }

    void OnGUI() {
        
        int points = EditorGUILayout.IntField("Number of points:", 5);
        if (GUILayout.Button("Create!")) {
            Type[] components = new Type[] { typeof(Trail) };
            GameObject go = new GameObject("Trail", components);
            Trail trail= go.GetComponent<Trail>();
            trail.points = new List<TrailPoint>();
            for (var i = 0; i < points; i++)
            {
                GameObject p = new GameObject("Point" + i);
                Transform t = p.transform;
                t.parent = trail.transform;

            }
        }
    }

} 