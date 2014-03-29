using UnityEngine;
using System.Collections;

public class GroundChecker : MonoBehaviour {
    public PlayerMovementController controller;

    private void OnTriggerEnter2d() {
        controller.isGrounded = true;
    }

    public void OnTriggerExit() {
        controller.isGrounded = false;
    }
}
