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

    private void Awake() {
        GameObject go = gameObject;
        gob = new GroundObstacle() {
            position = go.transform.position,
            rotaion = go.transform.localEulerAngles,
            scale = go.transform.localScale
        };
        obstacle = gob;
    }

    private void SetEditableProperties() {
        properties.edits |= EditableProperties.Properties.Position;
        properties.edits |= EditableProperties.Properties.Rotation;
        properties.edits |= EditableProperties.Properties.Scale;

        if(subType == GroundObstacle.GroundType.Moving_Ground) {
            properties.edits |= EditableProperties.Properties.Speed;
            properties.edits |= EditableProperties.Properties.Start_End_Pos;
        }
    }
}
