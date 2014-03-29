using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class EventTriggerButton : MonoBehaviour {
    private static Dictionary<int, List<EventTriggerButton>> groups = new Dictionary<int, List<EventTriggerButton>>();
    [Range(0, 50)]
    public int group = 0;
    public bool playerOnly;
    public bool requiresHold;
    public bool destroyAfterPress;
    public EventButtonPressAnimation anim;
    public bool isPressed { get; set; }
    private bool canPress = true;

    public delegate void EventButtonEvent(EventTriggerButton button);
    public delegate void EventButtonGroupEvent(List<EventTriggerButton> buttons);
    private event EventButtonEvent OnButtonPress;
    public event EventButtonEvent onButtonPress {
        add { OnButtonPress += value; }
        remove { OnButtonPress -= value; }
    }

    private event EventButtonEvent OnButtonRelease;
    public event EventButtonEvent onButtonRelease {
        add { OnButtonRelease += value; }
        remove { OnButtonRelease -= value; }
    }

    private event EventButtonGroupEvent OnButtonGroupPress;
    public event EventButtonGroupEvent onButtonGroupPress {
        add { OnButtonGroupPress += value; }
        remove { OnButtonGroupPress -= value; }
    }



    private void Awake() {
        if(group != 0) {
            if(groups.ContainsKey(group)) {
                List<EventTriggerButton> list = groups[group];
                list.Add(this);
                groups.Add(group, list);
            } else {
                List<EventTriggerButton> list = new List<EventTriggerButton>();
                list.Add(this);
                groups.Add(group, list);
            }
        }
    }

    private void OnDisable() {
        if(group != 0) groups[group].Remove(this);
    }

    private void OnCollisionEnter2D(Collision2D col) {
        bool isPlayer = col.gameObject.tag.Equals("Player");
        if(playerOnly && !isPlayer) return;
        OnPress();
    }

    private void OnCollisionStay2D(Collision2D col) {

    }

    private void OnCollisionExit2D(Collision2D col) {
        bool isPlayer = col.gameObject.tag.Equals("Player");
        if(playerOnly && !isPlayer) return;
        if(requiresHold) OnRelease();
    }

    private void OnPress() {
        if(canPress) {
            isPressed = true;
            canPress = false;
            anim.PressAnimation(true);
            if(group == 0) {
                if(OnButtonPress != null) OnButtonPress(this);
            } else {
                List<EventTriggerButton> buttons = groups[group];
                for(int i = 0; i < buttons.Count; i++) {
                    if(!buttons[i].isPressed) return;
                }

                if(OnButtonGroupPress != null) OnButtonGroupPress(groups[group]);
            }
        }
    }

    private void OnRelease() {
        isPressed = false;
        anim.PressAnimation(false);
        if(OnButtonRelease != null) OnButtonRelease(this);
    }

    private void AnimComplete(bool down) {
        if(down && destroyAfterPress) Destroy(gameObject);
    }
}

[System.Serializable]
public class EventButtonPressAnimation {
    public GameObject go;
    public float animDuration = .3f;
    public float delay = 0f;
    public float pressedScale = 0f;
    public iTween.EaseType easeType;

    public void PressAnimation(bool down) {
        if(down) {
            iTween.ScaleTo(go.gameObject, iTween.Hash(
                "y", pressedScale,
                "delay", delay,
                "time", animDuration,
                "easetype", easeType,
                "oncomplete", "AnimComplete",
                "oncompleteparams", true
                ));
        } else {
            iTween.ScaleTo(go.gameObject, iTween.Hash(
                "y", 1f,
                "delay", delay,
                "time", animDuration,
                "easetype", easeType,
                "oncomplete", "AnimComplete",
                "oncompleteparams", false
                ));
        }
    }
}
