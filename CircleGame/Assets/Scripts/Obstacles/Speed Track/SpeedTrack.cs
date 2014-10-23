using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider2D))]
public class SpeedTrack : MonoBehaviour {
    private Transform trans;
    private Player player;
    private bool playerEntered;

    public float force = 30f;
    public bool alterGravity;
    public float gravityScale = .5f;

    private float origGravityScale;

    //private void Awake() {
    //    string[] parts = name.Split(new string[] { "_" }, System.StringSplitOptions.RemoveEmptyEntries);
    //    name = string.Empty;
    //    foreach(string s in parts) {
    //        if(name.Equals(string.Empty)) name += s;
    //        else name += " " + s;
    //    }
    //}

    private void Start() {
        trans = transform;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(!playerEntered) {
            playerEntered = true;
            player = col.GetComponent<Player>();
            if(alterGravity) {
                origGravityScale = player.body2D.gravityScale;
                player.body2D.gravityScale = gravityScale;
                player.moveController.canMove = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col) {
        //switch(direction) {
        //    case TrackDirection.Left: player.body2D.AddForce(-Vector2.right * force); break;
        //    case TrackDirection.Right: player.body2D.AddForce(Vector2.right * force); break;
        //    case TrackDirection.Up: player.body2D.AddForce(Vector2.up * force); break;
        //    case TrackDirection.Down: player.body2D.AddForce(-Vector2.up * force); break;
        //    default: break;
        //}

        if(player == null) player = col.GetComponent<Player>();

        player.trans.rotation = trans.rotation;
        player.body2D.AddForce(player.trans.up * force);
    }

    private void OnTriggerExit2D(Collider2D col) {
        playerEntered = false;
        if(player == null) player = col.GetComponent<Player>();

        if(alterGravity) {
            player.body2D.gravityScale = origGravityScale;
            player.moveController.canMove = true;
        }

        player = null;
    }
}
