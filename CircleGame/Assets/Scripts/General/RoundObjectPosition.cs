using UnityEngine;
using System.Collections;

public class RoundObjectPosition : MonoBehaviour {
    private Transform trans;

    private void Start() { trans = transform; }

    public void Reposition() {
        float x = Mathf.Round(trans.position.x);
        float y = Mathf.Round(trans.position.y);
        float z = Mathf.Round(trans.position.z);

        trans.position = new Vector3(x, y, z);
    }
}
