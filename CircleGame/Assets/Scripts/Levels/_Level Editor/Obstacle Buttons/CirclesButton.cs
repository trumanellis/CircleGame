using UnityEngine;
using System.Collections;

public class CirclesButton : MonoBehaviour {
    public GameObject circle;

    public void OnClick() {
        GameObject go = Instantiate(circle, (Vector2)Camera.main.transform.position, Quaternion.identity) as GameObject;
        go.AddComponent<EditableObstacle>().type = ObstacleType.Circle;
        go.AddComponent<BoxCollider>().size = new Vector3(4f, 4f, 0f);
    }
}
