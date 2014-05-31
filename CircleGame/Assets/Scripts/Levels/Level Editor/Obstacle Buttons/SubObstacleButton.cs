﻿using UnityEngine;
using System.Collections;

public class SubObstacleButton : MonoBehaviour {
    public GameObject obstacle;

    public ObstacleType type;
    public CircleObstacle.CircleType circleType = CircleObstacle.CircleType.None;
    public SpeedtrackObstacle.SpeedTrackType speedtrackType = SpeedtrackObstacle.SpeedTrackType.None;
    public GroundObstacle.GroundType groundType = GroundObstacle.GroundType.None;

    public void OnClick() {
        GameObject go = Instantiate(obstacle, (Vector2)Camera.main.transform.position, Quaternion.identity) as GameObject;
        go.transform.parent = LevelEditorManager.instance.obstaclesRoot;
        EditableObstacle eob = null;
        switch(type) {
            case ObstacleType.Circle:
                var cob = go.AddComponent<EditableCircleObstacle>();
                cob.subType = circleType;
                eob = cob;
                break;
            case ObstacleType.Ground:
                var grob = go.AddComponent<EditableGroundObstacle>();
                grob.subType = groundType;
                eob = grob;
                break;
            case ObstacleType.Gear:
                eob = go.AddComponent<EditableGear>();
                break;
            case ObstacleType.Speed_Track:
                var stob = go.AddComponent<EditableSpeedTrackObstacle>();
                stob.subType = speedtrackType;
                eob = stob;
                break;
            case ObstacleType.RedBall:
                eob = go.AddComponent<EditableRedBall>();
                break;
            case ObstacleType.Cannon:
            case ObstacleType.Trampoline:
            case ObstacleType.Water: break;
        }
        EditableObstacle.SetCurrentObject(eob);
    }
}
