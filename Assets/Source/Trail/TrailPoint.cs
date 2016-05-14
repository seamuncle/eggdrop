using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TrailPoint : MonoBehaviour
{
    public Vector3 position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(position, 0.4f);
    }

    void OnDestroy()
    {
       // This gets called a lot when game plays in editor
       // Debug.LogError("He's dead, Jim", gameObject);
        
        Transform parent = transform.parent;
        if (parent == null)
        {
            return;
        }
        
        Trail trail = parent.GetComponent<Trail>();
        if (trail == null)
        {
            return;
        }
        
        trail.points.Remove(this);
        
        trail.CleanChildren();
    }
}
