using UnityEngine;
using System.Collections;

public class EditableSpeedTrackObstacle : EditableObstacle {
    private SpeedtrackObstacle stob;
    public SpeedtrackObstacle.SpeedTrackType subType { get { return stob.subType; } set { stob.subType = value; } }

    protected override void Awake() {
        GameObject go = gameObject;
        stob = new SpeedtrackObstacle() {
            position = go.transform.position,
            rotation = go.transform.localEulerAngles,
            scale = go.transform.localScale
        };
        obstacle = stob;
        SetEditableProperties();
        base.Awake();
    }

    private void SetEditableProperties() {
        properties.edits = (EditableProperties.Properties.Position | EditableProperties.Properties.Rotation);
    }
}
