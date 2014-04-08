using UnityEngine;
using System.Collections;

public class RadialMenuButton : MonoBehaviour {
    private bool pressed;
    private bool pressedStaged;

    private void OnDragOver() { pressedStaged = true; }
    private void OnDragOut() { pressedStaged = false; }
    private void Update() {
        if(pressed && Input.GetMouseButtonUp(1)) Debug.Log(gameObject.name);

        pressed = pressedStaged;
    }
}
