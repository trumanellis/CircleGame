using UnityEngine;
using System.Collections;

public class MoveCameraInEditMode : MonoBehaviour {
    public bool shouldTrack { get; set; }
    private Transform root;
    private Vector3 startPos;

    private void Awake() { root = transform; }

    private void Update() {
        if(Input.GetMouseButtonDown(2)) {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            shouldTrack = true;
        } else if(Input.GetMouseButtonUp(2)) shouldTrack = false;

        if(shouldTrack) {
            root.position -= (Camera.main.ScreenToWorldPoint(Input.mousePosition) - startPos);
        }
    }
}
