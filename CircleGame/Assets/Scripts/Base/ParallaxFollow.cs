using UnityEngine;
using System.Collections;

public class ParallaxFollow : MonoBehaviour {
    private Transform _trans;
    public Transform _target;
    [Range(0f, 1f)]
    public float _xPara = .5f;
    [Range(0f, 1f)]
    public float _yPara = .5f;
    public bool _shouldFollow = true;

    private Vector3 _lastPos;

    private void Start() {
        _trans = transform;

        if(_target == null) {
            Debug.LogWarning("Target is null");
            enabled = false;
        } else {
            _trans.position = _target.position;
            _lastPos = _target.position;
        }
    }

    private void LateUpdate() {
        if(_shouldFollow && _target.position != _lastPos) {
            var delta = _target.position - _lastPos;
            _trans.position += new Vector3(delta.x * _xPara, delta.y * _yPara, 0f);
            _lastPos = _target.position;
        }
    }

    public void SetTarget(Transform target) {
        this._target = target;
        if(_target != null) {
            _lastPos = _target.position;
            enabled = true;
        } else enabled = false;
    }
}
