using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class RedBall : MonoBehaviour {
    private bool hasPlayer;
    private Transform trans;
    private Player player;
    private float origGrav;

    public float gravityModifier;
    public float attractionForce = .2f;

    private void Awake() {
        trans = transform;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(!hasPlayer && col.tag.Equals("Player")) {
            hasPlayer = true;
            player = col.GetComponent<Player>();
            origGrav = player.body2D.gravityScale;
            player.body2D.gravityScale = gravityModifier;
        }
    }

    private void OnTriggerStay2D(Collider2D col) {
        if(hasPlayer) {
            Debug.Log("Has Player");
            player.body2D.AddForce( ((Vector2)trans.position - (Vector2)player.trans.position).normalized * attractionForce );
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if(hasPlayer && col.tag.Equals("Player")) {
            hasPlayer = false;
            player.body2D.gravityScale = origGrav;
        }
    }
}
