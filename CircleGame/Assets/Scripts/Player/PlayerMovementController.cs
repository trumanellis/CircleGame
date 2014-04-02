using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {
    private Player player;
    public Transform groundCheckerRoot;
    public Transform[] groundCheckers;
    public TrailRenderer speedTrail;
    public float trailThreshHoldX = 3;
    public float trailThreshHoldY = 6;
    public float groundedMoveForce = 40f;
    public float airbornMoveForce = 30f;
    public float slowingForce = 1.3f;
    public float maxSpeed = 2.6f;
    public float jumpForce = 200f;
    public float stoppingThreshHold = .2f;
    public bool isGrounded { get; set; }
    public bool canMove;
    public bool trailEnabled = true;
    private bool jump;
    private int collisionMask = 0;

    private void Awake() { 
        player = GetComponent<Player>();
        CreateCollisionMask();
    }

    private void Update() {
        isGrounded = false;

        for(int i = 0; i < groundCheckers.Length; i++) {
            if(Physics2D.Linecast(transform.position, groundCheckers[i].position, collisionMask)) {
                isGrounded = true;
                break;
            }
        }

        if(canMove && cInput.GetVirtualKeyDown("Jump") && isGrounded)
            jump = true;
    }

    private void FixedUpdate() {
        if(canMove) {
            float h = cInput.GetVirtualAxis("Horizontal");
            if(h > .01f) {
                if(player.body2D.velocity.x < maxSpeed) {
                    float v = player.body2D.velocity.x + ((isGrounded ? groundedMoveForce : airbornMoveForce) / player.body2D.mass) * Time.fixedDeltaTime - player.body2D.angularDrag;
                    if(v < maxSpeed) player.body2D.AddForce(Vector2.right * h * (isGrounded ? groundedMoveForce : airbornMoveForce));
                    else player.body2D.velocity = new Vector2(maxSpeed, player.body2D.velocity.y);
                }

            } else if(h < -.01f) {
                if(player.body2D.velocity.x > -maxSpeed) {
                    float v = player.body2D.velocity.x - ((isGrounded ? groundedMoveForce : airbornMoveForce) / player.body2D.mass) * Time.fixedDeltaTime - player.body2D.angularDrag;
                    if(v > -maxSpeed) player.body2D.AddForce(Vector2.right * h * (isGrounded ? groundedMoveForce : airbornMoveForce));
                    else player.body2D.velocity = new Vector2(-maxSpeed, player.body2D.velocity.y);
                }
            }

            if(jump) {
                player.body2D.AddForce(new Vector2(0f, jumpForce));
                jump = false;
            }
        }

        if(trailEnabled) {
            if(Mathf.Abs(player.body2D.velocity.x) > trailThreshHoldX) speedTrail.enabled = true;
            else if(Mathf.Abs(player.body2D.velocity.y) > trailThreshHoldY) speedTrail.enabled = true;
        }

        SlowDown();
        groundCheckerRoot.position = player.position;
    }

    private void SlowDown() {
        if(trailEnabled && Mathf.Abs(player.body2D.velocity.x) < trailThreshHoldX && isGrounded) speedTrail.enabled = false;

        if(player.body2D.velocity.x == 0 || !isGrounded) return;
        if(Mathf.Abs(player.body2D.velocity.x) < stoppingThreshHold) player.body2D.velocity = new Vector2(0f, player.body2D.velocity.y);
        if(player.body2D.velocity.x != 0) {
            bool right = player.body2D.velocity.x > 0;
            player.body2D.AddForce((right ? -Vector2.right : Vector2.right) * slowingForce);
        }
    }

    private void CreateCollisionMask() {
        collisionMask = (1 << LayerMask.NameToLayer("Ground") | (1 << LayerMask.NameToLayer("Cannon")) | (1 << LayerMask.NameToLayer("Trampoline")));
    }
}
