using UnityEngine;
using System.Collections;

public class ZoomCamera : MonoBehaviour {
    private void OnScroll(float delta) {
        LevelEditorCamera.Zoom(delta);
    }
}
