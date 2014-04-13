using UnityEngine;
using System.Collections;

public class GizmoFollow : MonoBehaviour {
    private Transform trans;
    public Transform obstacle { get; set; }

    private void Awake() { trans = transform; }

	private void Update () {
        trans.position = LevelEditorManager.uiCamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(obstacle.position));
	}
}
