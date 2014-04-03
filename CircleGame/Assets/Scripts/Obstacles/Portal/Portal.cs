using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Portal : MonoBehaviour {
    public Portal destinationPortal; 
    public bool receivingTarget { get; set; }

    private void OnTriggerEnter2D(Collider2D col) {
        if(!receivingTarget) {
            if(destinationPortal != null) {
                destinationPortal.receivingTarget = true;
                col.gameObject.transform.position = new Vector2(destinationPortal.transform.position.x, destinationPortal.transform.position.y);
            } else col.gameObject.transform.position = Vector2.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        receivingTarget = false;
    }
}
