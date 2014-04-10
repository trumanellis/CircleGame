using UnityEngine;
using System.Collections;

public class RadialMenuDeleteObstacle : MonoBehaviour {
    private RadialMenu manager;
    private bool pressed;
    private bool pressedStaged;

    private void Start() {
        manager = transform.parent.GetComponent<RadialMenu>();
    }

    private void OnDragOver() {
        pressedStaged = true;
        manager.OnMenuHover("Delete", true);
        SoundManager.Play("Button Hover");
    }

    private void OnDragOut() {
        pressedStaged = false;
        manager.OnMenuHover("Delete", false);
    }

    private void Update() {
        if(pressed && Input.GetMouseButtonUp(1)) MenuSelected();
        pressed = pressedStaged;
    }

    protected virtual void MenuSelected() { manager.DeleteRequested(); }
}
