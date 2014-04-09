using UnityEngine;
using System.Collections;

public class RadialMenuButton : MonoBehaviour {
    private RadialMenu manager;
    private TweenPosition[] tweens;
    private TweenPosition tween;
    private bool pressed;
    private bool pressedStaged;

    public string menuName { get; set; }

    private void Awake() { 
        manager = transform.parent.GetComponent<RadialMenu>();
        menuName = gameObject.name;
        //tweens = GetComponents<TweenPosition>();
        //for(int i = 0; i < tweens.Length; i++) {
        //    if(tweens[i].tweenGroup == 1) tween = tweens[i];
        //}
    }

    private void OnDragOver() { 
        pressedStaged = true; 
        manager.OnMenuHover(menuName, true);
        SoundManager.Play("Button Hover");
        //tween.PlayForward();
    }

    private void OnDragOut() {
        pressedStaged = false; 
        manager.OnMenuHover(menuName, false);
        //tween.PlayReverse();
    }

    private void Update() {
        if(pressed && Input.GetMouseButtonUp(1)) MenuSelected();
        pressed = pressedStaged;
    }

    protected virtual void MenuSelected() { manager.OnMenuSelected(name); }
}
