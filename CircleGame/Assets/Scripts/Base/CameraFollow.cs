using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    public static CameraFollow instance;
    public tk2dCamera cam { get; private set; }
    private Transform us;
    public Transform target1;
    public Transform target2;
    private Rigidbody body1;
    private Rigidbody2D body2D1;
    //private Rigidbody body2;
    //private Rigidbody2D body2D2;
    public bool allowZooming = true;
    public float followSpeed = 1.0f;
    public float zoomSpeedOut = 0.2f;
    public float zoomSpeedIn = 0.2f;
    public float minSpeedThreshhold = .2f;
    public float maxSpeedThreshhold = 3.0f;
    public float maxZoomFactor = 0.6f;
    public float startingZoom;
    public Vector2 offSet = Vector2.zero;
    public bool battleView = false;
    public float battleZoomFactor = 0.5f;
    public float MaxDistanceThreshold = 5;
    public float battleZoomSpeed = 0.5f;
    public float maxBattleZoom = 0.4f;
    public bool shouldFollow = true;

    private void Start() {
        instance = this;

        us = transform;
        cam = GetComponent<tk2dCamera>();
        startingZoom = cam.ZoomFactor;

        if(target1 != null) {
            body1 = target1.GetComponent<Rigidbody>();
            body2D1 = target1.GetComponent<Rigidbody2D>();
        }
    }

    private void FixedUpdate() {
        if(shouldFollow) {
            if(target1 != null) {
                if(!battleView) {
                    Vector3 start = us.position;
                    Vector3 end = Vector3.MoveTowards(start, target1.position + (Vector3)offSet, followSpeed * Time.deltaTime);
                    end.z = start.z;
                    us.position = end;

                    //i want the camera to be quick for zoom out and slower for zoom in;
                    if(allowZooming && (body1 != null || body2D1 != null) && cam != null) {
                        float speed = body2D1 != null ? body2D1.velocity.magnitude : body1.velocity.magnitude;
                        float speedSclamp = Mathf.Clamp01((speed - minSpeedThreshhold) / (maxSpeedThreshhold - minSpeedThreshhold));

                        float targetZoom = Mathf.Lerp(startingZoom, maxZoomFactor, speedSclamp);
                        var zoom = Mathf.MoveTowards(cam.ZoomFactor, targetZoom, (targetZoom < cam.ZoomFactor ? zoomSpeedOut : zoomSpeedIn) * Time.deltaTime);
                        cam.ZoomFactor = zoom;
                    }
                } else if(target2 != null) {
                    //find middle between 2 targets and center the camera on that
                    Vector3 start = us.position;
                    Vector3 end = Vector3.MoveTowards(start, ((target1.position + target2.position) * 0.5f) + (Vector3)offSet, followSpeed * Time.deltaTime);
                    end.z = start.z;
                    us.position = end;

                    //the zoom of the camera depends on the distance between the 2 targets
                    if(allowZooming) {
                        float distance = Vector3.Distance(target2.position, target1.position);
                        //float distanceClamp = Mathf.Clamp01((distance - minSpeedThreshhold) / (maxSpeedThreshhold - minSpeedThreshhold));
                        //float targetZoom = Mathf.Lerp(minZoom, maxZoomFactor, distanceClamp);

                        //var zoom = Mathf.MoveTowards(cam.ZoomFactor, targetZoom, (targetZoom < cam.ZoomFactor ? zoomSpeedOut : zoomSpeedIn) * Time.deltaTime);

                        cam.ZoomFactor = Mathf.Max(maxBattleZoom, Mathf.Clamp01(Mathf.MoveTowards(cam.ZoomFactor, distance / (Mathf.Pow(distance, 2) / battleZoomFactor), battleZoomSpeed * Time.deltaTime)));

                        Debug.Log(distance);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Remaps the value of val from range low1-high1 to be with range low2-high2
    /// </summary>
    /// <param name="val"></param>
    /// <param name="low1"></param>
    /// <param name="high1"></param>
    /// <param name="low2"></param>
    /// <param name="high2"></param>
    /// <returns></returns>
    public static float Remap(float val, float low1, float high1, float low2, float high2) {
        return low2 + (val - low1) * ((high2 - low2) / (high1 - low1));
    }

    /// <summary>
    /// Remaps the value of val from range low1-high1 to be with range low2-high2
    /// </summary>
    /// <param name="val"></param>
    /// <param name="low1"></param>
    /// <param name="high1"></param>
    /// <param name="low2"></param>
    /// <param name="high2"></param>
    /// <returns></returns>
    public static float RemapFlip(float val, float low1, float high1, float low2, float high2) {
        return high2 - (val - low1) * (high2 - low2) / (high1 - low1);
    }

    /// <summary>
    /// Remaps the value of val to be within 0f & 1f
    /// </summary>
    /// <param name="val"></param>
    /// <param name="low1"></param>
    /// <param name="high1"></param>
    /// <param name="low2"></param>
    /// <param name="high2"></param>
    /// <returns></returns>
    public static float RemapSimple(float val, float low1, float high1) {
        return 0f + (val - low1) * (1f - 0f) / (high1 - low1);
    }

    /// <summary>
    /// Remaps the value of val to be within 0f & 1f
    /// </summary>
    /// <param name="val"></param>
    /// <param name="low1"></param>
    /// <param name="high1"></param>
    /// <param name="low2"></param>
    /// <param name="high2"></param>
    /// <returns></returns>
    public static float RemapSimpleFlip(float val, float low1, float high1) {
        return 1f - (val - low1) * (1f - 0f) / (high1 - low1);
    }

    public void SetTarget(Transform target) {
        this.target1 = target;
        if(target1 != null) {
            body1 = target1.GetComponent<Rigidbody>();
            body2D1 = target1.GetComponent<Rigidbody2D>();
        } else {
            body1 = null;
            body2D1 = null;
        }
    }

    public void SetTarget2(Transform target) {
        this.target2 = target;
        //if(target2 != null) {
        //    body2 = target1.GetComponent<Rigidbody>();
        //    body2D2 = target1.GetComponent<Rigidbody2D>();
        //} else {
        //    body2 = null;
        //    body2D2 = null;
        //}
    }

    public void SwitchToBattleView(Transform t1, Transform t2) {
        SetTarget(t1);
        SetTarget2(t2);
        battleView = true;
    }

    public void SwitchToNormalView(Transform t1) {
        SetTarget(t1);
        battleView = false;
    }
}
