using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour {
    public enum DestroyAfterFallType { None, InTime, OnLand, OffScreen }
    private Rigidbody2D body2D;
    public float timeToFall = 1f;
    public float gravityScale = 1f;
    public DestroyAfterFallType destroyType = DestroyAfterFallType.None;
    public float destroyInTime = 3f;

    private void Start() {
        body2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag.Equals("Player")) StartCoroutine(Startfalling());
        else if(destroyType == DestroyAfterFallType.OnLand) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(!col.tag.Equals("Player") && destroyType == DestroyAfterFallType.OnLand) Destroy(gameObject);
    }

    private void OnBecameInvisible() {
        if(destroyType == DestroyAfterFallType.OffScreen) Destroy(gameObject);
    }

    private IEnumerator Startfalling() {

        yield return new WaitForSeconds(timeToFall);
        body2D.gravityScale = gravityScale;
        body2D.isKinematic = false;
        if(destroyType == DestroyAfterFallType.InTime) Destroy(gameObject, destroyInTime);
    }
}
