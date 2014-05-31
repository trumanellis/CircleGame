using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class EditableProperties {
    public Properties edits;
    public static Properties blankEdits = (Properties.Set_Player_Start | Properties.Set_Portal_Pos);

    [System.Flags]
    public enum Properties {
        //commons
        [EnumDescription("Re Position")]
        Position = 1 << 0,
        [EnumDescription("Rotate")]
        Rotation = 1 << 1,
        [EnumDescription("Scale")]
        Scale = 1 << 2,

        //blank properties
        [EnumDescription("Set Start")]
        Set_Player_Start = 1 << 3,
        [EnumDescription("Set Portal")]
        Set_Portal_Pos = 1 << 4,

        //uniques
        [EnumDescription("Change Speed")]
        Speed = 1 << 5, // cannon rotation/moving platforms
        [EnumDescription("Start/End Position")]
        Start_End_Pos = 1 << 6, // moving platforms
        [EnumDescription("Left/Right Bounds")]
        left_Right_Bounds = 1 << 7, //canons
        [EnumDescription("Toggle Ground")]
        Remove_Center_Ground = 1 << 8,
        [EnumDescription("Change Dir")]
        Toggle_Rotation_Direction = 1 << 9,
    }
}

public class EnumDescription : System.Attribute {
    public string displayName { get; private set; }

    public EnumDescription(string name) {
        displayName = name;
    }
}

public static class EditablePropertiesExtensions {
    const BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance;

    public static string GetDescription(this Enum prop) {
        Type type = prop.GetType();

        FieldInfo info = type.GetField(prop.ToString());
        object[] pas = info.GetCustomAttributes(typeof(EnumDescription), false);
        if(pas.Length > 0)
            return (EnumDescription)pas[0] == null ? prop.ToString() : ((EnumDescription)pas[0]).displayName;
        return prop.ToString();
    }

    public static EditableProperties.Properties[] GetProperties(this EditableProperties.Properties prop) {
        var props = new List<EditableProperties.Properties>((EditableProperties.Properties[])Enum.GetValues(typeof(EditableProperties.Properties)));
        for(int i = props.Count - 1; i >= 0; i--) {
            if(!prop.Contains(props[i])) props.Remove(props[i]);
        } return props.ToArray();
    }
    public static bool Contains(this EditableProperties.Properties main, EditableProperties.Properties prop) {
        return (main & prop) == prop;
    }
}
