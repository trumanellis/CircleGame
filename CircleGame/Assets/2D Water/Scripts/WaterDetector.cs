using UnityEngine;
using System.Collections;

public class WaterDetector : MonoBehaviour {
    private Water water;

    private void Awake() {
        water = transform.parent.GetComponent<Water>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.rigidbody2D != null) {
            water.Splash(transform.position.x, col.rigidbody2D.velocity.y * col.rigidbody2D.mass / 40f);
        }
    }
}
