using UnityEngine;
using System.Collections;

public class EditableRedBall : EditableObstacle {
    protected override void Awake() {
        GameObject go = gameObject;
        obstacle = new Obstacle() {
            position = go.transform.position,
            rotation = go.transform.localEulerAngles,
            scale = go.transform.localScale,
            obstacleType = ObstacleType.RedBall
        };
        SetEditableProperties();
        base.Awake();
    }

    private void SetEditableProperties() {
        properties.edits = EditableProperties.Properties.Position;
    }
}
