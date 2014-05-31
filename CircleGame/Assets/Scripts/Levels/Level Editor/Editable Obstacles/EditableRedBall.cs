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
        properties.edits = EditableProperties.Properties.Position | EditableProperties.Properties.Scale;
        uniformScale = true;
        maxScale = new Vector2(10f, 10f);
        minScale = new Vector2(1f, 1f);
    }
}
