using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour {
    public float multForce = 20f;
    public float maxPlayerVelocity = 50f;
    private static bool entered;

    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag.Equals("Player") && !entered) {
            entered = true;
            var player = col.collider.GetComponent<Player>();
            if(Mathf.Abs(player.body2D.velocity.y) < maxPlayerVelocity) {
                player.body2D.AddForce(transform.up * multForce);
            }
        }
    }

    private void OnCollisionExit2D() {
        entered = false;
    }
}
