using UnityEngine;
using System.Collections;

public class EditableObstacle : MonoBehaviour {
    private Transform trans;
    public Obstacle obstacle { get; set; }
    public ObstacleType type = ObstacleType.Circle;
    private iTween.EaseType ease = iTween.EaseType.easeInExpo;
    private float heldTime;
    private float showTime = .5f;
    private bool shouldCount;
    private bool shouldReposition;
    private bool showingMenu;

    private void Start() { trans = transform; }

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
            ShowMenu();
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
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            trans.position = (Vector2)pos;
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
        iTween.ScaleTo(gameObject, iTween.Hash(
            "name", "Scale Up",
            "time", showTime,
            "easetype", ease,
            "scale", new Vector3(1.1f, 1.1f, 1f)
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
