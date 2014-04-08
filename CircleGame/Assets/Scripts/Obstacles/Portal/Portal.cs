using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public delegate void PortalDelegate();
public class Portal : MonoBehaviour {
    public bool receivingTarget { get; set; }
    public event PortalDelegate onPlayerEnter;
    public event PortalDelegate onPlayerExit;

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag.Equals("Player") && !receivingTarget) {
            if(onPlayerEnter != null) onPlayerEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if(col.tag.Equals("Player") && onPlayerExit != null) onPlayerExit();
        receivingTarget = false;
    }
}
