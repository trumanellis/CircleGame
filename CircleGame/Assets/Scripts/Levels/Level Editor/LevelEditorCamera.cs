using UnityEngine;
using System.Collections;

public class LevelEditorCamera : MonoBehaviour {
    private static LevelEditorCamera instance;
    private Transform trans;
    private Vector3 startPos;
    private Vector3 cameraPos;
    public tk2dCamera mainCam;
    public tk2dCamera bgCam;
    public Vector4 worldBounds;
    private Vector4? currentCameraBounds;
    public float accelerant = 1.0f;
    public float zoomSpeed = 1.0f;
    public float maxZoom = .5f;
    public bool shouldTrack { get; set; }

    private void Awake() {
        instance = this;
        trans = transform;
        currentCameraBounds = worldBounds;
    }

    private void Update() {
        if(Input.GetMouseButtonDown(2) || ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftAlt)) && Input.GetMouseButtonDown(0))) {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            shouldTrack = true;
        } else if(Input.GetMouseButtonUp(2) || (Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.RightAlt)) || Input.GetMouseButtonUp(0)) shouldTrack = false;

        if(shouldTrack) {
            trans.position -= (Camera.main.ScreenToWorldPoint(Input.mousePosition) - startPos);
            RepositionCamera();
        }
    }

    public static void Zoom(float scroll) {
        if(scroll != 0) {
            float startZoom = instance.mainCam.ZoomFactor;
            if(instance.mainCam.ZoomFactor + scroll < instance.maxZoom) {
                instance.mainCam.ZoomFactor = instance.maxZoom;
                instance.bgCam.ZoomFactor = instance.maxZoom;
            } else if(instance.mainCam.ZoomFactor + scroll > 1f) {
                instance.mainCam.ZoomFactor = 1f;
                instance.bgCam.ZoomFactor = 1f;
            } else {
                instance.mainCam.ZoomFactor += scroll;
                instance.bgCam.ZoomFactor += scroll;

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                instance.trans.position += mousePos;
            }

            if(instance.mainCam.ZoomFactor != startZoom) {
                Vector4 newBounds = Vector4.zero;

                newBounds.x = instance.worldBounds.x * instance.mainCam.ZoomFactor;
                newBounds.y = instance.worldBounds.y * instance.mainCam.ZoomFactor;
                newBounds.z = instance.worldBounds.z * instance.mainCam.ZoomFactor;
                newBounds.w = instance.worldBounds.w * instance.mainCam.ZoomFactor;

                instance.currentCameraBounds = newBounds;
            }
            instance.RepositionCamera();
        }
    }

    public void RepositionCamera() {
        cameraPos = transform.position;

        if(cameraPos.x < currentCameraBounds.Value.x) cameraPos.x = currentCameraBounds.Value.x;
        else if(cameraPos.x > currentCameraBounds.Value.z) cameraPos.x = currentCameraBounds.Value.z;

        if(cameraPos.y > currentCameraBounds.Value.y) cameraPos.y = currentCameraBounds.Value.y;
        else if(cameraPos.y < currentCameraBounds.Value.w) cameraPos.y = currentCameraBounds.Value.w;

        cameraPos.z = -10;
        trans.position = cameraPos;
    }
}
