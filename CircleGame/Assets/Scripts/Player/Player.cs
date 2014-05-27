using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public Transform root { get; private set; }
    public Transform trans { get; private set; }
    public SpriteRenderer spriteRend { get; private set; }
    public Rigidbody2D body2D { get; private set; }
    public Collider2D col2D { get; private set; }
    public PlayerMovementController moveController { get; private set; }
    public Vector2 position { get { return trans.position; } set { trans.position = value; } }
    public float standardGravityScale { get; private set; }

    public float alpha { 
        get { return spriteRend.color.a; }
        set { spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, value); }
    }

	private void Start () {
        trans = transform;
        root = trans.parent;
        body2D = rigidbody2D;
        col2D = collider2D;
        spriteRend = GetComponent<SpriteRenderer>();
        moveController = GetComponent<PlayerMovementController>();
        standardGravityScale = body2D.gravityScale;
	}
}
