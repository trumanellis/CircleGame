using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class EditableProperties {
    public Properties edits;
    public BlankProperties blankEdits = BlankProperties.None;

    public EditableProperties() {
        blankEdits |= BlankProperties.Set_Player_Start;
        blankEdits |= BlankProperties.Set_Portal_Pos;
    }

    public Properties[] GetProperties() {
        var props = new List<Properties>((Properties[])Enum.GetValues(typeof(Properties)));
        for(int i = props.Count - 1; i > 0; i--) {
            if(!edits.Contains(props[i])) props.Remove(props[i]);
        }

        return props.ToArray();
    }

    [System.Flags]
    public enum Properties : byte {
        [DisplayName("RePosition")]
        Position = 1 << 0,
        [DisplayName("Rotate")]
        Rotation = 1 << 1,
        [DisplayName("Scale")]
        Scale = 1 << 2,

        [DisplayName("Change Speed")]
        Speed = 1 << 3, // cannon rotation/moving platforms
        [DisplayName("Start/End Position")]
        Start_End_Pos = 1 << 4, // moving platforms
        [DisplayName("Left/Right Bounds")]
        left_Right_Bounds = 1 << 5, //canons
        [DisplayName("Toggle Ground")]
        Remove_Center_Ground = 1 << 6
    }

    [System.Flags]
    public enum BlankProperties : byte {
        None = 0,
        Set_Player_Start = 1 << 0,
        Set_Portal_Pos = 1 << 1
    }
}

public class DisplayNameAttribute : System.Attribute {
    public string displayName { get; private set; }

    public DisplayNameAttribute(string name) {
        displayName = name;
    }
}

public static class EditablePropertiesExtensions {
    const BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance;

    public static string GetDescription(this EditableProperties.Properties prop) {
        Type type = prop.GetType();

        FieldInfo info = type.GetField(prop.ToString());
        DisplayNameAttribute pa = info.GetCustomAttributes(typeof(DisplayNameAttribute), false)[0] as DisplayNameAttribute;
        return pa == null ? prop.ToString() : pa.displayName;
    }

    public static bool Contains(this EditableProperties.Properties main, EditableProperties.Properties prop) {
        return (main & prop) == prop;
    }
}
