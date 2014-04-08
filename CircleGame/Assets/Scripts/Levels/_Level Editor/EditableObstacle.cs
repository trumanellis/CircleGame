using UnityEngine;
using System.Collections;

public class EditableObstacle : MonoBehaviour {
    private Transform trans;
    private tk2dCamera cam;
    public Obstacle obstacle { get; set; }
    public ObstacleType type = ObstacleType.Circle;
    private iTween.EaseType ease = iTween.EaseType.easeInExpo;
    private BoxCollider boundingBox;
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

    private void OnPress(bool pressed) {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) {
            shouldCount = pressed;
            if(pressed) StartScaling();
            else if(!pressed) {
                StopScaling();
            }
        }
    }

    private void OnClick() {
        if(Input.GetMouseButtonUp(1))
            LevelEditorManager.ShowRadialMenu(type);
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

        if(shouldReposition){
            Vector3 pos = Input.mousePosition;
            if(pos.x > Screen.width - (boundingBox.size.x / 2f) * (100f * cam.ZoomFactor)) pos.x = Screen.width - (boundingBox.size.x / 2f) * (100f * cam.ZoomFactor) - leftPadding;
            else if(pos.x < (boundingBox.size.x / 2f) * (100f * cam.ZoomFactor)) pos.x = (boundingBox.size.x / 2f) * (100f * cam.ZoomFactor) + rightPadding;
            if(pos.y > Screen.height - (boundingBox.size.y / 2f) * (100f * cam.ZoomFactor)) pos.y = Screen.height - (boundingBox.size.y / 2f) * (100f * cam.ZoomFactor) - upperPadding;
            if(pos.y < (boundingBox.size.y / 2f) * (100f * cam.ZoomFactor)) pos.y = (boundingBox.size.y / 2f) * (100f * cam.ZoomFactor) + lowerPadding;

            trans.position = (Vector2)Camera.main.ScreenToWorldPoint(pos);
            obstacle.position = trans.position;
            obstacle.scale = trans.localScale;
            obstacle.rotaion = trans.eulerAngles;
        }
    }

    private void ShowMenu() {
        showingMenu = !showingMenu;

        //show available menus for item
        Debug.Log((showingMenu ? "Should show" : "Closing") + " menu for " + type);
    }

    private void StartScaling() {
        iTween.ColorTo(gameObject, iTween.Hash(
            "name", "Scale Up",
            "time", showTime,
            "easetype", ease,
            "color", Color.green
            ));
    }

    private void StopScaling() {
        heldTime = 0f;
        shouldReposition = false;
        shouldCount = false;
        gameObject.transform.localScale = Vector3.one;
        iTween.StopByName("Scale Up");
    }
}
