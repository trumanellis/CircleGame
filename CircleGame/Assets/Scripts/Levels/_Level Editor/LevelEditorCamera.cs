using UnityEngine;
using System.Collections;

public class LevelEditorCamera : MonoBehaviour {
    private Transform root;
    private Vector3 startPos;
    public tk2dCamera mainCam;
    public tk2dCamera bgCam;
    public float accelerant = 1.0f;
    public float zoomSpeed = 1.0f;
    public float maxZoom = .5f;
    public bool shouldTrack { get; set; }

    private void Awake() { root = transform; }

    private void Update() {
        float scroll = cInput.GetAxis("Vertical");
        if(mainCam.ZoomFactor + scroll < maxZoom) {
            mainCam.ZoomFactor = maxZoom;
            bgCam.ZoomFactor = maxZoom;
        } else if(mainCam.ZoomFactor + scroll > 1) {
            mainCam.ZoomFactor = 1f;
            bgCam.ZoomFactor = 1f;
        } else {
            mainCam.ZoomFactor += scroll;
            bgCam.ZoomFactor += scroll;
        }

        if(Input.GetMouseButtonDown(2) || ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftAlt)) && Input.GetMouseButtonDown(0))) {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            shouldTrack = true;
        } else if(Input.GetMouseButtonUp(2) || (Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.RightAlt)) || Input.GetMouseButtonUp(0)) shouldTrack = false;

        if(shouldTrack) {
            root.position -= (Camera.main.ScreenToWorldPoint(Input.mousePosition) - startPos);
        }
    }
}
