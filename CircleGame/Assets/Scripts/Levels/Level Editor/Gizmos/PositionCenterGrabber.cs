﻿using UnityEngine;
using System.Collections;

public class PositionCenterGrabber : MonoBehaviour {
    public Gizmo gizmo;

    private void OnDrag(Vector2 delta) {
        gizmo.OnDrag(delta);
    }
}