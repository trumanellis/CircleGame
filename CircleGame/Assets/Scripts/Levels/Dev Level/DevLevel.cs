using UnityEngine;
using System.Collections;

public class DevLevel : MonoBehaviour {
    public Player player;
    public CameraFollow followingCam;
    public EventTriggerArea eventTrigger;

    private void Start() {
        player.moveController.canMove = false;
        player.moveController.remoteControlled = true;
        followingCam.cam.ZoomFactor = .7f;
        followingCam.allowZooming = false;

        cInput.PressVirtualKey("Remote Left");

        eventTrigger.onAreaEnter += (a, go) => {
            cInput.ReleaseVirtualKey("Remote Left");
            iTween.ColorTo(player.gameObject, iTween.Hash(
                "color", Color.black,
                "time", .5f,
                "oncompletetarget", gameObject,
                "oncomplete", "FadeComplete"
                ));

            iTween.ValueTo(gameObject, iTween.Hash(
                "from", followingCam.cam.ZoomFactor,
                "to", 1,
                "time", .5f,
                "easetype", iTween.EaseType.linear,
                "onupdate", "ChangeZoomValue"
                ));
        };

        eventTrigger.onAreaExit += (a, go) => {
            eventTrigger.gameObject.collider2D.isTrigger = false;
            eventTrigger.isEnabled = false;
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
