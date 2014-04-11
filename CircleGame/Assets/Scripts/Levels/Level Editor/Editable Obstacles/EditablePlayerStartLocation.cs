using UnityEngine;
using System.Collections;

public class EditablePlayerStartLocation : EditableObstacle {
    protected override void Awake() {
        obstacle = new Obstacle() {
            obstacleType = ObstacleType.Player_Start,
            position = transform.position,
            rotaion = transform.eulerAngles,
            scale = transform.localScale
        };

        SetEditableProperties();
        base.Awake();
    }

    public void SetLocation(Vector2 location) {
        transform.position = location;
        obstacle.position = transform.position;
    }

    private void SetEditableProperties() {
        properties.edits = EditableProperties.Properties.Position;
    }
}
