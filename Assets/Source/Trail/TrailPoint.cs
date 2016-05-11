using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

}
