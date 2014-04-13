using UnityEngine;
using System.Collections;

public class EditPropertyUIController : MonoBehaviour {
    public ScaleGizmo _scaleGizmo;
    public PositionGizmo _positionGizmo;
    public static ScaleGizmo scaleGizmo;
    public static PositionGizmo positionGizmo;

    private void Awake() {
        positionGizmo = _positionGizmo;
        scaleGizmo = _scaleGizmo;

        positionGizmo.gameObject.SetActive(false);
        scaleGizmo.gameObject.SetActive(false);
    }
}
