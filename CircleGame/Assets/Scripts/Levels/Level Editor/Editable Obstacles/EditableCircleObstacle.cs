using UnityEngine;
using System.Collections;

public class EditableCircleObstacle : EditableObstacle {
    private CircleObstacle cob;
    public CircleObstacle.CircleType subType { get { return cob.subType; } set { cob.subType = value; } }

    protected override void Awake() {
        GameObject go = gameObject;
        cob = new CircleObstacle() {
            position = go.transform.position,
            rotaion = go.transform.localEulerAngles,
            scale = go.transform.localScale
        };
        obstacle = cob;
        SetEditableProperties();
        base.Awake();
    }

    private void SetEditableProperties() {
        properties.edits = (EditableProperties.Properties.Position | EditableProperties.Properties.Remove_Center_Ground);
    }

    public override void EditProperty(EditableProperties.Properties prop) {
        if(properties.edits.Contains(prop)) {
            if(prop == EditableProperties.Properties.Remove_Center_Ground) ToggleGround();
            else base.EditProperty(prop);
        }
    }

    public void ToggleGround() {
        GameObject ground = transform.Find("Ground").gameObject;
        ground.SetActive(!ground.activeSelf);
        cob.showGround = ground.activeSelf;
    }
}
