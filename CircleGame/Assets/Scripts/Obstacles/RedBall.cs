using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class RedBall : MonoBehaviour {
    private bool hasPlayer;
    private Transform trans;
    private Player player;

    public float gravityModifier;
    public float attractionForce = .2f;

    private void Awake() {
        trans = transform;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(!hasPlayer && col.tag.Equals("Player")) {
            hasPlayer = true;
            player = col.GetComponent<Player>();
        }
    }

    private void OnTriggerStay2D(Collider2D col) {
        if(hasPlayer) {
            player.body2D.gravityScale = gravityModifier;
            player.body2D.AddForce(((Vector2)trans.position - (Vector2)player.trans.position).normalized * attractionForce);
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if(hasPlayer && col.tag.Equals("Player")) {
            hasPlayer = false;
            player.body2D.gravityScale = player.standardGravityScale;
        }
    }
}
