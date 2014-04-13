using UnityEngine;
using System.Collections;

public class ScaleCenterGrabber : MonoBehaviour {
    public ScaleGizmo gizmo;

    private void OnDrag(Vector2 delta) {
        float largest = Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y));
        gizmo.OnDrag(new Vector2(Mathf.Sign(delta.x) * largest, Mathf.Sign(delta.y) * largest));
    }
}
