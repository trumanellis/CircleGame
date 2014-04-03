using UnityEngine;
using System.Collections;

public class CameraFollow: MonoBehaviour {
    public tk2dCamera cam { get; private set; }
    private Transform us;
    public Transform target;
    private Rigidbody body;
    private Rigidbody2D body2D;
    public float followSpeed = 1.0f;

    public bool allowZooming = true;
    public float minZoomSpeed = 20.0f;
    public float maxZoomSpeed = 40.0f;
    public float maxZoomFactor = 0.6f;

    private void Start() {
        us = transform;
        cam = GetComponent<tk2dCamera>();
        Init();
    }

    private void FixedUpdate() {
        if(target != null) {
            Vector3 start = us.position;
            Vector3 end = Vector3.MoveTowards(start, target.position, followSpeed * Time.deltaTime);
            end.z = start.z;
            us.position = end;

            if(allowZooming && (body != null || body2D != null) && cam != null) {
                float spd = body2D != null ? body2D.velocity.magnitude : body.velocity.magnitude;
                float scl = Mathf.Clamp01((spd - minZoomSpeed) / (maxZoomSpeed - minZoomSpeed));
                float targetZoomFactor = Mathf.Lerp(1, maxZoomFactor, scl);
                cam.ZoomFactor = Mathf.MoveTowards(cam.ZoomFactor, targetZoomFactor, 0.2f * Time.deltaTime);
            }
        }
    }

    private void Init() {
        if(target != null) {
            body = target.GetComponent<Rigidbody>();
            body2D = target.GetComponent<Rigidbody2D>();
        }
    }

    public void SetTarget(Transform target) {
        this.target = target;
        Init();
    }
}
