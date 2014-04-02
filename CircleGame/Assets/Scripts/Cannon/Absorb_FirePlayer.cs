using UnityEngine;
using System.Collections;

public class Absorb_FirePlayer : MonoBehaviour {
    private Player player;
    public Transform cannonTip;
    private Transform cannon;
    public CameraFollow followingCamera;
    public float force = 200;
    public float cameraTweenDureation = 1.5f;
    public float maxZoom = .6f;
    public float cameraZoomDelay = .5f;
    public iTween.EaseType easeType;
    private bool hasPlayer;
    private bool playerFired;
    private bool canFire;
    public bool allowPlayerfire = true;
    public bool shouldFire { get; set; }

    public delegate void Cannondelegate();
    private event Cannondelegate OnPlayerEnter;
    public event Cannondelegate onPlayerEnter {
        add { OnPlayerEnter += value; }
        remove { OnPlayerEnter -= value; }
    }

    private event Cannondelegate OnFirePlayer;
    public event Cannondelegate onFirePlayer {
        add { OnFirePlayer += value; }
        remove { OnFirePlayer -= value; }
    }

    private void Awake() { cannon = transform.parent; }

    private void OnTriggerEnter2D(Collider2D col) {
        if(!hasPlayer && !playerFired) {
            hasPlayer = true;
            player = col.GetComponent<Player>();
            player.body2D.gravityScale = 0;
            if(OnPlayerEnter != null) OnPlayerEnter();

            if(followingCamera != null) {
                followingCamera.allowZooming = false;
                iTween.ValueTo(gameObject, iTween.Hash(
                    "name", "Camera Zoom",
                    "delay", cameraZoomDelay,
                    "from", followingCamera.cam.ZoomFactor,
                    "to", maxZoom,
                    "easetype", easeType,
                    "onupdate", "ChangeZoomValue"
                    ));
            }
        }
    }

    public void FiredPlayer() {
        player.body2D.gravityScale = 1;
        playerFired = false;
        canFire = false;
        player = null;
        shouldFire = false;
        if(OnFirePlayer != null) OnFirePlayer();
    }

    private void Update() {
        if(hasPlayer && !playerFired) {
            player.position = cannonTip.position;
            if(!canFire && (cInput.GetVirtualKeyDown("Jump") || shouldFire)) {
                canFire = true;
            } else if(canFire && (cInput.GetVirtualKeyUp("Jump") || shouldFire)) {
                StartCoroutine(LaunchPlayer());
            }
        }
    }

    private IEnumerator LaunchPlayer() {
        player.trans.rotation = cannon.rotation;
        player.body2D.velocity = player.trans.up * force;
        hasPlayer = false;
        playerFired = true;

        if(followingCamera != null) {
            followingCamera.allowZooming = true;
            iTween.StopByName("Camera Zoom");
        }

        yield return new WaitForSeconds(.1f); // this is here due to a unity bug with 2d triggers not sending right callbacks
        FiredPlayer();
    }

    private void ChangeZoomValue(float value) {
        followingCamera.cam.ZoomFactor = value;
    }
}
