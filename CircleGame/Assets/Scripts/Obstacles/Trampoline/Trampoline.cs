using UnityEngine;
using System.Collections;

public class Trampoline : MonoBehaviour {
    public float multForce = 20f;
    public float maxPlayerVelocity = 50f;

    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag.Equals("Player")) {
            Player player = col.collider.GetComponent<Player>();
            if(Mathf.Abs(player.body2D.velocity.x) > maxPlayerVelocity || Mathf.Abs(player.body2D.velocity.y) > maxPlayerVelocity) {
                player.body2D.AddForce(player.body2D.velocity * multForce);
            } 
        }
    }
}
