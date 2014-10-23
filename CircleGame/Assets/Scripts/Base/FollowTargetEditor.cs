using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FollowTargetEditor : MonoBehaviour {
    public Transform target;
    public bool shouldFollow;
    private Transform us;
    private Vector3 delta;

    public bool lockY;
    public bool lockX;
    public bool lockZ;

    private void Start() {
        Init();
    }

    private void Init() {
        us = transform;
    }

    private void Update() {
        if(shouldFollow && target != null && !Application.isPlaying) {
            var newPos = target.position - delta;
            us.position = new Vector3((lockX ? us.position.x : newPos.x), (lockY ? us.position.y : newPos.y), (lockZ ? us.position.z : newPos.z));
        }
    }

    public void RecalculateDelta() {
        if(target != null) {
            delta = target.position - us.position;
            enabled = true;
        } else {
            Debug.LogWarning("The target is null");
            enabled = false;
        }
    }
}
