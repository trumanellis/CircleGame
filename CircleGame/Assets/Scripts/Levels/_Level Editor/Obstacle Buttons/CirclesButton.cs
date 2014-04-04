using UnityEngine;
using System.Collections;

public class CirclesButton : MonoBehaviour {
    public GameObject circle;
    public LevelEditorManager manager;

    public void OnClick() {
        GameObject go = Instantiate(circle, (Vector2)Camera.main.transform.position, Quaternion.identity) as GameObject;
        go.AddComponent<BoxCollider>().size = new Vector3(4f, 4f, 0f);
        EditableObstacle editob = go.AddComponent<EditableObstacle>();
        editob.type = ObstacleType.Circle;
        CircleObstacle cob = new CircleObstacle();
        cob.type = CircleObstacle.CircleType.Circle_Triple;
        cob.position = go.transform.position;
        cob.scale = go.transform.localScale;
        cob.rotaion = go.transform.eulerAngles;
        editob.obstacle = cob;
        manager.AddObstacle(cob);
        go.transform.parent = manager.gameObject.transform;
    }
}
