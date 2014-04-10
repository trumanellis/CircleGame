using UnityEngine;
using System.Collections;

public class RadialMenuButton : MonoBehaviour {
    private RadialMenu manager;
    private TweenPosition[] tweens;
    private TweenPosition tween;
    private bool pressed;
    private bool pressedStaged;

    public EditableProperties.Properties property { get; set; }

    private void Start() { 
        manager = transform.parent.GetComponent<RadialMenu>();
        //tweens = GetComponents<TweenPosition>();
        //for(int i = 0; i < tweens.Length; i++) {
        //    if(tweens[i].tweenGroup == 1) tween = tweens[i];
        //}
    }

    private void OnDragOver() { 
        pressedStaged = true;
        manager.OnMenuHover(property.GetDescription(), true);
        SoundManager.Play("Button Hover");
        //tween.PlayForward();
    }

    private void OnDragOut() {
        pressedStaged = false;
        manager.OnMenuHover(property.GetDescription(), false);
        //tween.PlayReverse();
    }

    private void Update() {
        if(pressed && Input.GetMouseButtonUp(1)) MenuSelected();
        pressed = pressedStaged;
    }

    protected virtual void MenuSelected() { manager.OnMenuSelected(property); }
}
