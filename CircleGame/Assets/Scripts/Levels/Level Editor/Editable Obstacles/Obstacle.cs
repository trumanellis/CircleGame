using UnityEngine;
using System.Collections;

/// <summary>
/// This class is used as a wrapper for the items that users have access to in the level creator.
/// It is used for the serialization/deserialization of obstacles
/// </summary>

public enum ObstacleType {
    None,
    Circle,
    Ground,
    Water,
    [EnumDescription("Speed Track")]
    Speed_Track,
    Cannon,
    Trampoline,
    [EnumDescription("Player Start")]
    Player_Start,
    [EnumDescription("Portal Position")]
    Portal_Position,
    [EnumDescription("Red Ball")]
    RedBall,
    Gear
}
public class Obstacle {
    public ObstacleType obstacleType = ObstacleType.None;
    public Vector3 position { get; set; }
    public Vector3 scale { get; set; }
    public Vector3 rotation { get; set; }
}

public class CircleObstacle : Obstacle {
    public enum CircleType {
        None,
        [EnumDescription("Single Circle")]
        Circle_Single,
        [EnumDescription("Double Circle")]
        Circle_Double,
        [EnumDescription("Triple Circle")]
        Circle_Triple,
    }
    public CircleType subType = CircleType.None;
    public bool showGround = true;

    public CircleObstacle() { obstacleType = ObstacleType.Circle; }
}

public class GroundObstacle : Obstacle {
    public enum GroundType {
        None,
        Ground,
        [EnumDescription("Falling Ground")]
        Falling_Ground,
        [EnumDescription("Moving Ground")]
        Moving_Ground,
        Triangle,
        Trampoline
    }
    public GroundType subType = GroundType.None;

    public GroundObstacle() { obstacleType = ObstacleType.Ground; }
}

public class SpeedtrackObstacle : Obstacle {
    public enum SpeedTrackType {
        None,
        [EnumDescription("Speed Track (small)")]
        Small,
        [EnumDescription("Speed Track (wide)")]
        Wide,
    }
    public SpeedTrackType subType = SpeedTrackType.None;

    public SpeedtrackObstacle() { obstacleType = ObstacleType.Speed_Track; }
}

public class WaterObstacle : Obstacle {
    public float height = 2;
    public float width = 10;
    public int percision = 5;
}

public class GearObstacle : Obstacle {
    public CircleRotation.RotationDirection direction = CircleRotation.RotationDirection.Left;
    public float rotationSpeed;
}

#region obstacle prefabs
[System.Serializable]
public class CirlePrefabs {
    public GameObject circleSingle;
    public GameObject circleDouble;
    public GameObject circleTriple;
    public GameObject gear;
}

[System.Serializable]
public class GroundPrefabs {
    public GameObject ground;
    public GameObject fallingGround;
    public GameObject movingGround;
    public GameObject trampoline;
    public GameObject triGround;
}

[System.Serializable]
public class SpeedTrackPrefabs {
    public GameObject small;
    public GameObject wide;
}
#endregion
