using UnityEngine;
using System.Collections;

public class PositionGrabber : MonoBehaviour {
    private TweenColor tween;
    public PositionGizmo gizmo;
    private bool isX;

    private void Awake() {
        tween = GetComponent<TweenColor>();
        isX = transform.parent.name.StartsWith("X");
    }

    private void OnPress(bool pressed) {
        if(pressed) tween.PlayForward();
        else tween.PlayReverse();
    }

    private void OnDrag(Vector2 delta) {
        gizmo.OnDrag(new Vector2((isX ? delta.x : 0f), isX ? 0f : delta.y));
    }
}
