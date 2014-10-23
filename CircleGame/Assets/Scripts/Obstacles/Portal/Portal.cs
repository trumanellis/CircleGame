using UnityEngine;
using System.Collections;

public delegate void PortalDelegate();
[RequireComponent(typeof(Collider2D))]
public class Portal : MonoBehaviour {
    public bool receivingTarget { get; set; }
    public event PortalDelegate onPlayerEnter;
    public event PortalDelegate onPlayerExit;

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag.Equals("Player") && !receivingTarget) {
            ActivatePortal(col.GetComponent<Player>());
            if(onPlayerEnter != null) onPlayerEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if(col.tag.Equals("Player") && onPlayerExit != null) onPlayerExit();
        receivingTarget = false;
    }

    private void ActivatePortal(Player player) {
        player.col2D.isTrigger = true;
        player.moveController.canMove = false;
        player.body2D.gravityScale = 0f;
        player.body2D.isKinematic = true;
        player.body2D.isKinematic = false; //turn on then off for a "reset"
        player.body2D.AddForce(Vector3.up * 100);
        player.levelWon = true;

        Camera.main.GetComponent<CameraFollow>().target1 = null;

        //should fade to black then show the thank you screen
    }
}
