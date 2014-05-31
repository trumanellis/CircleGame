using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class EventTriggerArea : MonoBehaviour {
    public bool playerOnly;
    public bool occupied { get; set; }
    public bool isEnabled { get; set; }

    public delegate void EventTriggerAreaEvent(EventTriggerArea area, GameObject obj);
    private event EventTriggerAreaEvent OnAreaEnter;
    public event EventTriggerAreaEvent onAreaEnter {
        add { OnAreaEnter += value; }
        remove { OnAreaEnter -= value; }
    }

    private event EventTriggerAreaEvent OnAreaExit;
    public event EventTriggerAreaEvent onAreaExit {
        add { OnAreaExit += value; }
        remove { OnAreaExit -= value; }
    }

    private void Awake() { isEnabled = true; }

    private void OnTriggerEnter2D(Collider2D col) {
        if(isEnabled) {
            bool isPlayer = col.gameObject.tag.Equals("Player");
            if(playerOnly && !isPlayer) return;

            occupied = true;
            if(OnAreaEnter != null) OnAreaEnter(this, col.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if(isEnabled) {
            bool isPlayer = col.gameObject.tag.Equals("Player");
            if(playerOnly && !isPlayer) return;

            occupied = false;
            if(OnAreaExit != null) OnAreaExit(this, col.gameObject);
        }
    }
}
