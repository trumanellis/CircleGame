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
                heldTime = 0f;
                shouldReposition = false;
                gameObject.transform.localScale = Vector3.one;
                iTween.StopByName("Scale Up");
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
        }
    }

    private void ShowMenu() {
        showingMenu = true;
        shouldCount = false;
        heldTime = 0f;

        //show available menus for item
        Debug.Log("Should show menu for " + type);
    }

    public void StartScaling() {
        iTween.ScaleTo(gameObject, iTween.Hash(
            "name", "Scale Up",
            "time", showTime,
            "easetype", ease,
            "scale", new Vector3(1.1f, 1.1f, 1f)
            ));
    }
}
