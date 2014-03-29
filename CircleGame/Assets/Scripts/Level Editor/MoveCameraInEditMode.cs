using UnityEngine;
using System.Collections;

public class MoveCameraInEditMode : MonoBehaviour {
    public float accelerant = 1.1f;
    public bool shouldTrack { get; set; }
    private Transform root;
    private Vector3 cameraStartPosition;
    private Vector2 startPosition;
    private Vector3 lastPosition;

    private void Awake() { root = transform; }

    private void Update() {
        //shouldTrack = true;
        //Vector3 mp = Input.mousePosition;
        //mp.z = trans.position.z;
        //startPosition = Camera.main.ScreenToWorldPoint(mp);
        //Debug.Log(startPosition);

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 delta = new Vector2(mouseWorld.x / 1f, mouseWorld.y / 1f);
        if(mouseWorld.x >= -960 && mouseWorld.x <= 960)
            root.position = new Vector3(-delta.x, root.position.y, 0);
        if(mouseWorld.y >= -540 && mouseWorld.y <= 540)
            root.position = new Vector3(root.position.x, -delta.y, 0);

    }
}
