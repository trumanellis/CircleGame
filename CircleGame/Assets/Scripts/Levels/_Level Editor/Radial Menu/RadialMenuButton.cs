using UnityEngine;
using System.Collections;

public class RadialMenuButton : MonoBehaviour {
    private RadialMenu manager;
    private bool pressed;
    private bool pressedStaged;

    private EditableProperties.Properties _property;
    public EditableProperties.Properties property {
        get { return _property; }
        set { 
            _property = value;
            propertyDescription = _property.GetDescription();
        }
    }

    public string propertyDescription { get; set; }

    private void Awake() { 
        manager = transform.parent.GetComponent<RadialMenu>();
        //tweens = GetComponents<TweenPosition>();
        //for(int i = 0; i < tweens.Length; i++) {
        //    if(tweens[i].tweenGroup == 1) tween = tweens[i];
        //}
    }

    private void OnDragOver() { 
        pressedStaged = true;
        manager.OnMenuHover(propertyDescription, true);
        SoundManager.Play("Button Hover");
        //tween.PlayForward();
    }

    private void OnDragOut() {
        pressedStaged = false;
        manager.OnMenuHover(propertyDescription, false);
        //tween.PlayReverse();
    }

    private void Update() {
        if(pressed && Input.GetMouseButtonUp(1)) MenuSelected();
        pressed = pressedStaged;
    }

    protected virtual void MenuSelected() { manager.OnMenuSelected(property); }
}
