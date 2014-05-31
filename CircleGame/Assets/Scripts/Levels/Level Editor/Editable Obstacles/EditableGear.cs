using UnityEngine;
using System.Collections;

public class EditableGear : EditableObstacle {
    protected override void Awake() {
        GameObject go = gameObject;
        obstacle = new GearObstacle() {
            position = go.transform.position,
            rotation = go.transform.localEulerAngles,
            scale = go.transform.localScale,
            obstacleType = ObstacleType.Gear
        };
        SetEditableProperties();
        base.Awake();
    }

    public override void EditProperty(EditableProperties.Properties prop) {
        if(properties.edits.Contains(prop)) {
            if(prop == EditableProperties.Properties.Toggle_Rotation_Direction) ChangeRotationDirection();
            else base.EditProperty(prop);
        }
    }

    private void ChangeRotationDirection() {
        var cr = gameObject.GetComponent<CircleRotation>();
        if(cr.direction == CircleRotation.RotationDirection.Left) cr.direction = CircleRotation.RotationDirection.Right;
        else cr.direction = CircleRotation.RotationDirection.Left;

        ((GearObstacle)obstacle).direction = cr.direction;
    }

    private void SetEditableProperties() {
        properties.edits = EditableProperties.Properties.Position | EditableProperties.Properties.Scale | EditableProperties.Properties.Toggle_Rotation_Direction;
        uniformScale = true;
        maxScale = new Vector2(3f, 3f);
        minScale = new Vector2(.5f, .5f);
    }
}