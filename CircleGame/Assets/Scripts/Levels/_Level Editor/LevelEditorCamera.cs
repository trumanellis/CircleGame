using UnityEngine;
using System.Collections;

public class LevelEditorCamera : MonoBehaviour {
    private Transform root;
    private Vector3 startPos;
    private Vector3 cameraPos;
    public tk2dCamera mainCam;
    public tk2dCamera bgCam;
    public Vector4 cameraBounds;
    private Vector4 currentCameraBounds;
    public float accelerant = 1.0f;
    public float zoomSpeed = 1.0f;
    public float maxZoom = .5f;
    public bool shouldTrack { get; set; }

    private void Awake() { 
        root = transform;
        currentCameraBounds = cameraBounds;
    }

    private void Update() {
        float scroll = cInput.GetAxis("Vertical");
        if(scroll != 0) {
            float startZoom = mainCam.ZoomFactor;
            if(mainCam.ZoomFactor + scroll < maxZoom) {
                mainCam.ZoomFactor = maxZoom;
                bgCam.ZoomFactor = maxZoom;
            } else if(mainCam.ZoomFactor + scroll > 1f) {
                mainCam.ZoomFactor = 1f;
                bgCam.ZoomFactor = 1f;
            } else {
                mainCam.ZoomFactor += scroll;
                bgCam.ZoomFactor += scroll;
            }

            if(mainCam.ZoomFactor < startZoom) currentCameraBounds = cameraBounds * mainCam.ZoomFactor;
            else if(mainCam.ZoomFactor > startZoom) currentCameraBounds = cameraBounds / mainCam.ZoomFactor;
        }

        if(Input.GetMouseButtonDown(2) || ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftAlt)) && Input.GetMouseButtonDown(0))) {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            shouldTrack = true;
        } else if(Input.GetMouseButtonUp(2) || (Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.RightAlt)) || Input.GetMouseButtonUp(0)) shouldTrack = false;

        if(shouldTrack)
            cameraPos = root.position -= (Camera.main.ScreenToWorldPoint(Input.mousePosition) - startPos);

        if(cameraPos.x < currentCameraBounds.x) cameraPos.x = currentCameraBounds.x;
        else if(cameraPos.x > currentCameraBounds.z) cameraPos.x = currentCameraBounds.z;

        if(cameraPos.y > currentCameraBounds.y) cameraPos.y = currentCameraBounds.y;
        else if(cameraPos.y < currentCameraBounds.w) cameraPos.y = currentCameraBounds.w;

        cameraPos.z = -10;
        root.position = cameraPos;
    }
}
