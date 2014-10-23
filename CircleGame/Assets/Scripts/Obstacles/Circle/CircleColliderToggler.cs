using UnityEngine;
using System.Collections;

public class CircleColliderToggler : MonoBehaviour {
    private PolygonCollider2D[] colliders;

    // Use this for initialization
    private void Start() {
        colliders = transform.GetComponentsInChildren<PolygonCollider2D>();
    }

    private void OnBecameInvisible() {
        for(int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = false;
    }

    private void OnBecameVisible() {
        for(int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = true;
    }
}
