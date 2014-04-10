using UnityEngine;
using System.Collections;

public class EditableCircleObstacle : EditableObstacle {
    private CircleObstacle cob;
    public CircleObstacle.CircleType subType { get { return cob.subType; } set { cob.subType = value; } }

    private void Awake() {
        GameObject go = gameObject;
        cob = new CircleObstacle() {
            position = go.transform.position,
            rotaion = go.transform.localEulerAngles,
            scale = go.transform.localScale
        };
        obstacle = cob;
        SetEditableProperties();
    }

    private void SetEditableProperties() {
        properties.edits |= EditableProperties.Properties.Position;
        properties.edits |= EditableProperties.Properties.Remove_Center_Ground;
    }
}
