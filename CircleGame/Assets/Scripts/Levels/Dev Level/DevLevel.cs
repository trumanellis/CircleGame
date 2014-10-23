using UnityEngine;
using System.Collections;

public class DevLevel : MonoBehaviour {
    public Player player;
    public CameraFollow followingCam;
    public EventTriggerArea eventTrigger;
    public Portal portal;

    public float startingZoom = .6f;
    public static float startingZoomFactor;
    private float originalZoom;

    private void Start() {
        startingZoomFactor = startingZoom;
        originalZoom = followingCam.cam.ZoomFactor;
        followingCam.allowZooming = false;
        followingCam.cam.ZoomFactor = startingZoom;

        if(portal != null) portal.onPlayerEnter += () => { Application.LoadLevel("Thanks"); };

        eventTrigger.onAreaEnter += (a, go) => {
            cInput.PressVirtualKey("Remote Right");

            player.moveController.canMove = false;
            player.moveController.remoteControlled = true;

            //cInput.ReleaseVirtualKey("Remote Left");
            iTween.ColorTo(player.gameObject, iTween.Hash(
                "color", Color.black,
                "time", .5f,
                "oncompletetarget", gameObject,
                "oncomplete", "FadeComplete"
                ));

            iTween.ValueTo(gameObject, iTween.Hash(
                "from", followingCam.cam.ZoomFactor,
                "to", originalZoom,
                "time", .5f,
                "easetype", iTween.EaseType.linear,
                "onupdate", "ChangeZoomValue"
                ));
        };

        eventTrigger.onAreaExit += (a, go) => {
            cInput.ReleaseVirtualKey("Remote Right");

            eventTrigger.gameObject.collider2D.isTrigger = false;
        };
    }

    private void FadeComplete() {
        player.moveController.canMove = true;
        player.moveController.remoteControlled = false;
    }

    private void ChangeZoomValue(float value) {
        followingCam.cam.ZoomFactor = value;
    }
}
