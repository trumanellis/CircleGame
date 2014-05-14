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
    Portal_Position
}
public class Obstacle {
    public ObstacleType obstacleType = ObstacleType.Circle;
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
        Circle_Triple
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
