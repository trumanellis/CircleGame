using UnityEngine;
using System.Collections;

public class CircleRotation : MonoBehaviour {
    public enum RotationDirection { Left, Right }
    private Transform circle;
    public float rotationSpeed = 30f;
    public RotationDirection direction = RotationDirection.Left;

	// Use this for initialization
	private void Awake() {
        circle = transform;
	}
	
	// Update is called once per frame
	private void FixedUpdate() {
        circle.Rotate(new Vector3(0f, 0f, direction == RotationDirection.Left ? 1f : -1f) * rotationSpeed * Time.deltaTime);
	}
}
