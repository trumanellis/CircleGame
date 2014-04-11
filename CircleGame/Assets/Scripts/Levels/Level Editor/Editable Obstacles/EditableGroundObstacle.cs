using UnityEngine;
using System.Collections;

public class EditableGroundObstacle : EditableObstacle {
    private GroundObstacle gob;
    public GroundObstacle.GroundType subType {
        get { return gob.subType; } 
        set { 
            gob.subType = value; 
            SetEditableProperties(); 
        }
    }

    protected override void Awake() {
        GameObject go = gameObject;
        gob = new GroundObstacle() {
            position = go.transform.position,
            rotaion = go.transform.localEulerAngles,
            scale = go.transform.localScale
        };
        obstacle = gob;
        base.Awake();
    }

    private void SetEditableProperties() {
        properties.edits = (EditableProperties.Properties.Position | EditableProperties.Properties.Rotation | EditableProperties.Properties.Scale);

        if(subType == GroundObstacle.GroundType.Moving_Ground)
            properties.edits |= (EditableProperties.Properties.Speed | EditableProperties.Properties.Start_End_Pos);
        else if(subType == GroundObstacle.GroundType.Trampoline)
            properties.edits ^= EditableProperties.Properties.Scale;
    }
}
