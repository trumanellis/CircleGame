using UnityEngine;
using System.Collections;

public class EditPropertyUIController : MonoBehaviour {
    //public UIButton _altButton;
    //public UISlider _horizontalSlider;
    //public UISlider _verticleSlider;

    //public static UIButton altButton;
    //public static UISlider horizontalSlider;
    //public static UISlider verticleSlider;

    public GameObject _scaleGizmo;
    public GameObject _positionGizmo;
    public static GameObject scaleGizmo;
    public static GameObject positionGizmo;

    private void Awake() {
        //altButton = _altButton;
        //horizontalSlider = _horizontalSlider;
        //verticleSlider = _verticleSlider;
        positionGizmo = _positionGizmo;
        scaleGizmo = _scaleGizmo;

        positionGizmo.SetActive(false);
        scaleGizmo.SetActive(false);
    }
}
