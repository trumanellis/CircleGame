using UnityEngine;
using System.Collections;

public class ScaleCenterGrabber : MonoBehaviour {
    public Gizmo gizmo;

    private void OnDrag(Vector2 delta) {
        gizmo.OnDrag(Mathf.Sign(LargestKeepSign(delta.x, delta.y)) * Vector2.one);
    }

    private float LargestKeepSign(float x, float y) {
        float sx = Mathf.Sign(x);
        float sy = Mathf.Sign(y);
        float ax = Mathf.Abs(x);
        float ay = Mathf.Abs(y);

        if(ax >= ay) return ax * sx;
        else return ay * sy;
    }
}
