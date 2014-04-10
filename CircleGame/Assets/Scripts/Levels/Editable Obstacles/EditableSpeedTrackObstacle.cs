using UnityEngine;
using System.Collections;

public class EditableSpeedTrackObstacle : EditableObstacle {
    private SpeedtrackObstacle stob;
    public SpeedtrackObstacle.SpeedTrackType subType { get { return stob.subType; } set { stob.subType = value; } }

    private void Awake() {
        GameObject go = gameObject;
        stob = new SpeedtrackObstacle() {
            position = go.transform.position,
            rotaion = go.transform.localEulerAngles,
            scale = go.transform.localScale
        };
        obstacle = stob;
        SetEditableProperties();
    }

    private void SetEditableProperties() {
        properties.edits |= EditableProperties.Properties.Position;
        properties.edits |= EditableProperties.Properties.Rotation;
    }
}
