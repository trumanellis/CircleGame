using UnityEngine;
using System.Collections;

public class ScaleGizmo : MonoBehaviour {
    public Transform obstacle { get; set; }
    public delegate void OnGizmoDrag(Vector2 delta);
    public OnGizmoDrag onGizmoDrag;

    public void OnDrag(Vector2 delta) {
        if(onGizmoDrag != null) onGizmoDrag(delta);
    }
}
