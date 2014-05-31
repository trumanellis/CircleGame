using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CustomLevelManager : MonoBehaviour {
    public static LevelEditorManager instance { get; private set; }
    public static bool fromEditor { get; set; }
    private Transform root;

    public Transform player;
    public CameraFollow follow;
    public GameObject editorButton;
    public CirlePrefabs circlePrefabs;
    public GroundPrefabs groundPrefabs;
    public SpeedTrackPrefabs trackPrefabs;
    public GameObject redBall;

    private void Start() {
        root = transform;
        if(PlayerPrefs.HasKey("Serialize Test"))
            SaveOldLevel();
        LoadLevel(CreatedLevel.LoadLevel("Test Level"));
        follow.SetTarget(player);
    }

    private void SaveOldLevel() {
        CreatedLevel.SaveLevel("Test Level", PlayerPrefs.GetString("Serialize Test"));
    }

    private void LoadLevel(string levelData) {
        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        if(string.IsNullOrEmpty(levelData)) return;
        var obs = JsonConvert.DeserializeObject<List<Obstacle>>(levelData, settings);
        for(int i = 0; i < obs.Count; i++) {
            Vector3 pos = obs[i].position;
            Vector3 scale = obs[i].scale;
            Vector3 rot = obs[i].rotation;
            Transform trans = null;

            switch(obs[i].obstacleType) {
                case ObstacleType.Circle:
                    var cob = obs[i] as CircleObstacle;
                    if(cob.subType == CircleObstacle.CircleType.Circle_Single) trans = ((GameObject)Instantiate(circlePrefabs.circleSingle)).transform;
                    else if(cob.subType == CircleObstacle.CircleType.Circle_Double) trans = ((GameObject)Instantiate(circlePrefabs.circleDouble)).transform;
                    else trans = ((GameObject)Instantiate(circlePrefabs.circleTriple)).transform;

                    if(!cob.showGround)
                        trans.Find("Ground").gameObject.SetActive(false);
                    break;
                case ObstacleType.Ground:
                    var grob = obs[i] as GroundObstacle;
                    if(grob.subType == GroundObstacle.GroundType.Ground) trans = ((GameObject)Instantiate(groundPrefabs.ground)).transform;
                    else if(grob.subType == GroundObstacle.GroundType.Falling_Ground) trans = ((GameObject)Instantiate(groundPrefabs.fallingGround)).transform;
                    else if(grob.subType == GroundObstacle.GroundType.Moving_Ground) trans = ((GameObject)Instantiate(groundPrefabs.movingGround)).transform;
                    else if(grob.subType == GroundObstacle.GroundType.Trampoline) trans = ((GameObject)Instantiate(groundPrefabs.trampoline)).transform;
                    else if(grob.subType == GroundObstacle.GroundType.Triangle) trans = ((GameObject)Instantiate(groundPrefabs.triGround)).transform;
                    break;
                case ObstacleType.Gear:
                    var gob = obs[i] as GearObstacle;
                    trans = ((GameObject)Instantiate(circlePrefabs.gear)).transform;
                    trans.gameObject.GetComponent<CircleRotation>().direction = gob.direction;
                    break;
                case ObstacleType.Speed_Track:
                    var stob = obs[i] as SpeedtrackObstacle;
                    if(stob.subType == SpeedtrackObstacle.SpeedTrackType.Small) trans = ((GameObject)Instantiate(trackPrefabs.small)).transform;
                    else if(stob.subType == SpeedtrackObstacle.SpeedTrackType.Wide) trans = ((GameObject)Instantiate(trackPrefabs.wide)).transform;
                    break;
                case ObstacleType.RedBall:
                    trans = ((GameObject)Instantiate(redBall)).transform;
                    break;
                case ObstacleType.Player_Start: trans = player; break;
                default: break;
            }
            if(trans != null) {
                trans.position = pos;
                trans.eulerAngles = rot;
                trans.localScale = scale;

                if(trans != player) trans.parent = root;
            }
        }
    }

    public void ReturnToEditor() {
        Application.LoadLevel("Level Editor");
    }

    public void RestartLevel() {
        Application.LoadLevel(Application.loadedLevelName);
    }

    private void OnDestroy() { fromEditor = false; }
}
