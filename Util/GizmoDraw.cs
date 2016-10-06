using UnityEngine;
using System.Collections;

public class GizmoDraw : MonoBehaviour {
    public float size=0.5f;
    public Color gizmoColor;
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(this.gameObject.transform.position, size);
    }
}
