using UnityEngine;
using System.Collections;

public class CannonRotater : MonoBehaviour {
    private Transform cannon;
    private bool left;
    private float currentRot;
    public float rotationSpeed;
    public float leftBound;
    public float rightBound;

    private void Awake() { cannon = transform; }

    //private void Start() { leftBouond = 360 - leftBouond; rightBound = 360 - rightBound; }

    // Update is called once per frame
    private void Update() {
        if(left && currentRot < leftBound) left = !left;
        else if(!left && currentRot < rightBound) left = !left;

        cannon.Rotate(new Vector3(0f, 0f, left ? 1f : -1f) * rotationSpeed * Time.deltaTime);
        if(currentRot < leftBound || currentRot < rightBound) {

            cannon.Rotate(new Vector3(0f, 0f, left ? 1f : -1f) *
                (!left ? (Mathf.Abs(180 - cannon.rotation.eulerAngles.z) - rightBound) : (Mathf.Abs(180 - cannon.rotation.eulerAngles.z) - leftBound)));

            //Debug.Log("After Fix: " + Mathf.Abs(180 - cannon.rotation.eulerAngles.z));
        }
        currentRot = Mathf.Abs(180 - cannon.rotation.eulerAngles.z);

        //if(currentRot < leftBound || currentRot < rightBound)
        //    Debug.Log("Before Fix: " + Mathf.Abs(180 - cannon.rotation.eulerAngles.z));
    }
}
