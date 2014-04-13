using UnityEngine;
using System.Collections;

public class RotationGrabber : MonoBehaviour {
    private TweenColor tween;
    public Gizmo gizmo;

    private void Awake() { tween = GetComponent<TweenColor>(); }

    private void OnPress(bool pressed) {
        if(pressed) tween.PlayForward();
        else tween.PlayReverse();
    }

    private void OnDrag(Vector2 delta) {
        gizmo.OnDrag(new Vector2(0f, delta.y));
    }
}
