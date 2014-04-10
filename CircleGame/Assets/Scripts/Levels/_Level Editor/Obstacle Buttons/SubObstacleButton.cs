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

        Obstacle ob = null;
        switch(type) {
            case ObstacleType.Circle:
                var cob = go.AddComponent<EditableCircleObstacle>();
                cob.subType = circleType;
                ob = cob.obstacle;
                break;
            case ObstacleType.Ground:
                var gob = go.AddComponent<EditableGroundObstacle>();
                gob.subType = groundType;
                ob = gob.obstacle;
                break;
            case ObstacleType.Speed_Track:
                var stob = go.AddComponent<EditableSpeedTrackObstacle>();
                stob.subType = speedtrackType;
                ob = stob.obstacle;
                break;
            case ObstacleType.Cannon:
            case ObstacleType.Trampoline:
            case ObstacleType.Water: break;
        }

        manager.AddObstacle(ob);
    }
}
