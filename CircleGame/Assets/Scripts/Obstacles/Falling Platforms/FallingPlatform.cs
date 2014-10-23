using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour {
    public enum DestroyAfterFallType { None, InTime, OnLand, OffScreen }
    private Rigidbody2D body2D;
    private Vector3 startingPosition;
    private Quaternion startingRotation;
    public float timeToFall = 1f;
    public float gravityScale = 1f;
    public DestroyAfterFallType destroyType = DestroyAfterFallType.None;
    public float destroyInTime = 3f;
    public bool resetOnFall;
    public bool _active = true;

    private void Start() {
        body2D = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag.Equals("Player")) {
            if(col.gameObject.GetComponent<Player>().position.y > transform.position.y && _active)
                StartCoroutine(Startfalling());
        } else if(destroyType == DestroyAfterFallType.OnLand) {
            if(resetOnFall) StopFalling();
            else Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(!col.tag.Equals("Player") && destroyType == DestroyAfterFallType.OnLand) {
            Debug.Log("Test");
            if(resetOnFall) StopFalling();
            else Destroy(gameObject);
        }
    }

    private void OnBecameInvisible() {
        if(destroyType == DestroyAfterFallType.OffScreen) {
            if(resetOnFall) StopFalling();
            else Destroy(gameObject);
        }
    }

    private IEnumerator Startfalling() {
        yield return new WaitForSeconds(timeToFall);
        body2D.gravityScale = gravityScale;
        body2D.isKinematic = false;
        if(destroyType == DestroyAfterFallType.OffScreen) collider2D.isTrigger = true;
        if(destroyType == DestroyAfterFallType.InTime) {
            Destroy(gameObject, destroyInTime);
        }
    }

    private void StopFalling() {
        body2D.gravityScale = 0f;
        body2D.isKinematic = true;
        transform.position = startingPosition;
        transform.rotation = startingRotation;
        if(destroyType == DestroyAfterFallType.OffScreen) collider2D.isTrigger = false;
    }
}
