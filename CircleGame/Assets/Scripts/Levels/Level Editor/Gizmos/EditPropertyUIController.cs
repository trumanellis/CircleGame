using UnityEngine;
using System.Collections;

public class EditPropertyUIController : MonoBehaviour {
    public Gizmo _scaleGizmo;
    public Gizmo _positionGizmo;
    public Gizmo _rotationGizmo;
    public static Gizmo scaleGizmo;
    public static Gizmo positionGizmo;
    public static Gizmo rotationGizmo;

    private void Awake() {
        positionGizmo = _positionGizmo;
        scaleGizmo = _scaleGizmo;
        rotationGizmo = _rotationGizmo;

        positionGizmo.gameObject.SetActive(false);
        scaleGizmo.gameObject.SetActive(false);
        rotationGizmo.gameObject.SetActive(false);
    }
}
