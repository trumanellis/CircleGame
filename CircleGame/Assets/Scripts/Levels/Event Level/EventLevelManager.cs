using UnityEngine;
using System.Collections;

public class EventLevelManager : MonoBehaviour {
    public EventTriggerArea eventTrigger;
    public EventTriggerArea playAgainPortal;
    public Transform levelStartPosition;
    public Transform levelEnd;
    public float zoomFactor = 1;

    private Vector3 offSet;
    private float startingZoomFactor;

    public CameraFollow followCam;

    // Use this for initialization
    private void Start() {
        if(eventTrigger != null) {
            eventTrigger.onAreaExit += (t, p) => { t.collider2D.isTrigger = false; };
            eventTrigger.onAreaEnter += (t, p) => {
                //t.collider2D.isTrigger = false;

                followCam.SetTarget(levelEnd);
                offSet = followCam.offSet;
                startingZoomFactor = followCam.startingZoom;
                followCam.offSet = Vector2.zero;
                followCam.startingZoom = 1;


                //followCam.shouldFollow = false;

                var player = p.GetComponent<Player>();

                iTween.ColorTo(player.gameObject, iTween.Hash(
                    "color", Color.white,
                    "time", .5f
                    ));

                iTween.ValueTo(gameObject, iTween.Hash(
                    "from", followCam.cam.ZoomFactor,
                    "to", zoomFactor,
                    "time", .5f,
                    "easetype", iTween.EaseType.linear,
                    "onupdate", "ChangeZoomValue"
                    ));
            };
        }


        if(playAgainPortal != null) playAgainPortal.onAreaEnter += (t, p) => {
            var player = p.GetComponent<Player>();

            followCam.offSet = offSet;
            followCam.startingZoom = startingZoomFactor;
            followCam.SetTarget(p.transform);
            followCam.transform.position = levelStartPosition.position;
            followCam.cam.ZoomFactor = DevLevel.startingZoomFactor;
            player.body2D.isKinematic = true;
            player.trans.position = new Vector3(levelStartPosition.position.x, levelStartPosition.position.y , 1F);
            player.body2D.isKinematic = false;
            eventTrigger.collider2D.isTrigger = true;
        };
    }

    private void ChangeZoomValue(float value) {
        followCam.cam.ZoomFactor = value;
    }
}
