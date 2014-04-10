using UnityEngine;
using System.Collections;

public class EditableObstacle : MonoBehaviour {
    private Transform trans;
    private tk2dCamera cam;
    private BoxCollider boundingBox;
    private iTween.EaseType ease = iTween.EaseType.easeInExpo;

    public static EditableObstacle currentObstacle { get; set; }
    public readonly EditableProperties properties = new EditableProperties();
    public Obstacle obstacle { get; protected set; }
    private float heldTime;
    private float showTime = .5f;
    private bool shouldCount;
    private bool shouldReposition;
    private bool showingMenu;

    private const float leftPadding = 5f;
    private const float rightPadding = 5f;
    private const float upperPadding = 5f;
    private const float lowerPadding = 5f;

    private void Start() {
        trans = transform;
        boundingBox = (BoxCollider)collider;
        cam = Camera.main.GetComponent<tk2dCamera>();
    }

    private void OnScroll(float delta) { LevelEditorManager.instance.editorCam.Zoom(delta); }
    private void OnPress(bool pressed) {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) {
            shouldCount = pressed;
            if(!pressed) {
                StopScaling();
            }
        }

        if(!SOS.isMobile) {
            if(!pressed) RadialMenu.instance.HideRadialMenu();
            else if(pressed && Input.GetMouseButton(1)) {
                RadialMenu.instance.ShowRadialMenu(this);
            }
        }
    }

    private void Update() {
        if(shouldCount && !showingMenu) {
            if(heldTime < showTime) heldTime += Time.deltaTime;
            else {
                shouldReposition = true;
                shouldCount = false;
                heldTime = 0f;
            }
        }

        if(shouldReposition) {
            Vector3 pos = Input.mousePosition;
            if(pos.x > Screen.width - (boundingBox.size.x / 2f) * (100f * cam.ZoomFactor) - leftPadding) pos.x = Screen.width - (boundingBox.size.x / 2f) * (100f * cam.ZoomFactor) - leftPadding;
            else if(pos.x < (boundingBox.size.x / 2f) * (100f * cam.ZoomFactor) + rightPadding) pos.x = (boundingBox.size.x / 2f) * (100f * cam.ZoomFactor) + rightPadding;
            if(pos.y > Screen.height - (boundingBox.size.y / 2f) * (100f * cam.ZoomFactor) - upperPadding) pos.y = Screen.height - (boundingBox.size.y / 2f) * (100f * cam.ZoomFactor) - upperPadding;
            else if(pos.y < (boundingBox.size.y / 2f) * (100f * cam.ZoomFactor) + lowerPadding) pos.y = (boundingBox.size.y / 2f) * (100f * cam.ZoomFactor) + lowerPadding;

            trans.position = (Vector2)Camera.main.ScreenToWorldPoint(pos);
            obstacle.position = trans.position;
        }
    }

    private void StopScaling() {
        heldTime = 0f;
        shouldReposition = false;
        shouldCount = false;
    }
}
