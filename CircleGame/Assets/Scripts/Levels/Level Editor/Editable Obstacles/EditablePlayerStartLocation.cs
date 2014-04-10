using UnityEngine;
using System.Collections;

public class EditablePlayerStartLocation : EditableObstacle {
    private void Start() {
        obstacle = new Obstacle() {
            obstacleType = ObstacleType.Player_Start,
            position = transform.position,
            rotaion = transform.eulerAngles,
            scale = transform.localScale
        };

        SetEditableProperties();
        LevelEditorManager.AddObstacle(obstacle);
    }

    public void SetLocation(Vector2 location) {
        transform.position = location;
        obstacle.position = transform.position;
    }

    private void SetEditableProperties() {
        properties.edits = EditableProperties.Properties.Position;
    }
}
