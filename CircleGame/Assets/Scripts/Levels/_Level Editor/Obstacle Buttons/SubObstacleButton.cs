using UnityEngine;
using System.Collections;

public class SubObstacleButton : MonoBehaviour {
    public GameObject obstacle;
    public LevelEditorManager manager;

    public ObstacleType type;
    public CircleObstacle.CircleType circleType = CircleObstacle.CircleType.None;
    public SpeedtrackObstacle.SpeedTrackType speedtrackType = SpeedtrackObstacle.SpeedTrackType.None;
    public GroundObstacle.GroundType groundType = GroundObstacle.GroundType.None;

    public void OnClick() {
        GameObject go = Instantiate(obstacle, (Vector2)Camera.main.transform.position, Quaternion.identity) as GameObject;

        EditableObstacle editob = go.AddComponent<EditableObstacle>();
        editob.manager = manager;
        editob.obstacle = new Obstacle() {

        };
    }
}
