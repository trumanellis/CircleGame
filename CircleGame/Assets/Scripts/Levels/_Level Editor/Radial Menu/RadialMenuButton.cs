using UnityEngine;
using System.Collections;

public class RadialMenuButton : MonoBehaviour {
    private RadialMenu manager;
    private bool pressed;
    private bool pressedStaged;

    public string menuName { get; set; }

    private void Awake() { manager = transform.parent.GetComponent<RadialMenu>(); menuName = gameObject.name; }

    private void OnDragOver() { pressedStaged = true; manager.OnMenuHover(menuName, true); }
    private void OnDragOut() { pressedStaged = false; manager.OnMenuHover(menuName, false); }
    private void Update() {
        if(pressed && Input.GetMouseButtonUp(1)) MenuSelected();
        pressed = pressedStaged;
    }

    protected virtual void MenuSelected() { manager.OnMenuSelected(name); }
}
