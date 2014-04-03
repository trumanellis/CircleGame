using UnityEngine;
using System.Collections;

public class LevelEditorCamera : MonoBehaviour {
    public bool shouldTrack { get; set; }
    private Transform root;
    private Vector3 startPos;
    public float accelerant = 1.0f;

    private void Awake() { root = transform; }

    private void Update() {
        if(Input.GetMouseButtonDown(2) || ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftAlt)) && Input.GetMouseButtonDown(0))) {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            shouldTrack = true;
        } else if(Input.GetMouseButtonUp(2) || (Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.RightAlt)) || Input.GetMouseButtonUp(0)) shouldTrack = false;

        if(shouldTrack) {
            root.position -= (Camera.main.ScreenToWorldPoint(Input.mousePosition) - startPos);
        }
    }
}
