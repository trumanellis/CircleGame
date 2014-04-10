using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class LevelEditorManager : MonoBehaviour {
    public static LevelEditorManager instance { get; private set; }
    private Transform root;
    private List<Obstacle> obstacles = new List<Obstacle>();
    public LevelEditorCamera editorCam;
    public RadialMenu radialMenu;

    public Color _selectedObstaColour;
    public static Color selectedObstacleColour;
    public CirlePrefabs circlePrefabs;
    public GroundPrefabs groundPrefabs;
    public SpeedTrackPrefabs trackPrefabs;

    private void Awake() {
        instance = this;
        root = transform;
        selectedObstacleColour = _selectedObstaColour;
    }

    private void Start() {
        if(PlayerPrefs.HasKey("Serialize Test")) {
            LoadLevel();
        }
    }

    public static void ShowRadialMenu(ObstacleType type) {
        instance.radialMenu.ShowRadialMenu(null);
    }

    public static void HideRadialMenu() {
        instance.radialMenu.HideRadialMenu();
    }

    private void OnScroll(float delta) {
        editorCam.Zoom(delta);
    }

    private void OnPress(bool pressed) {
        if(!SOS.isMobile) {
            if(!pressed) radialMenu.HideRadialMenu();
            else if(pressed && Input.GetMouseButtonDown(1)) {
                radialMenu.ShowRadialMenu(null);
                radialMenu.deleteButton.isEnabled = false;
            }
        }
    }

    private void LoadLevel() {
        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        string map = PlayerPrefs.GetString("Serialize Test");
        var obs = JsonConvert.DeserializeObject<List<Obstacle>>(map, settings);
        for(int i = 0; i < obs.Count; i++) {
            Vector3 pos = obs[i].position;
            Vector3 scale = obs[i].scale;
            Vector3 rot = obs[i].rotaion;
            Transform trans = null;

            switch(obs[i].obstacleType) {
                case ObstacleType.Circle:
                    var cob = obs[i] as CircleObstacle;
                    if(cob.subType == CircleObstacle.CircleType.Circle_Single) trans = ((GameObject)Instantiate(circlePrefabs.circleSingle, pos, Quaternion.identity)).transform;
                    else if(cob.subType == CircleObstacle.CircleType.Circle_Double) trans = ((GameObject)Instantiate(circlePrefabs.circleDouble, pos, Quaternion.identity)).transform;
                    else trans = ((GameObject)Instantiate(circlePrefabs.circleTriple, pos, Quaternion.identity)).transform;

                    var ecob = trans.gameObject.AddComponent<EditableCircleObstacle>();
                    ecob.subType = cob.subType;
                    obstacles.Add(ecob.obstacle);
                    break;
                case ObstacleType.Ground:
                    var gob = obs[i] as GroundObstacle;
                    if(gob.subType == GroundObstacle.GroundType.Ground) trans = ((GameObject)Instantiate(groundPrefabs.ground, pos, Quaternion.identity)).transform;
                    else if(gob.subType == GroundObstacle.GroundType.Falling_Ground) trans = ((GameObject)Instantiate(groundPrefabs.fallingGround, pos, Quaternion.identity)).transform;

                    var egob = trans.gameObject.AddComponent<EditableGroundObstacle>();
                    egob.subType = gob.subType;
                    obstacles.Add(egob.obstacle);
                    break;

                case ObstacleType.Speed_Track:
                    var stob = obs[i] as SpeedtrackObstacle;
                    if(stob.subType == SpeedtrackObstacle.SpeedTrackType.Small_Down) trans = ((GameObject)Instantiate(trackPrefabs.smallDown, pos, Quaternion.identity)).transform;
                    else if(stob.subType == SpeedtrackObstacle.SpeedTrackType.Small_Left) trans = ((GameObject)Instantiate(trackPrefabs.smallLeft, pos, Quaternion.identity)).transform;
                    else if(stob.subType == SpeedtrackObstacle.SpeedTrackType.Small_Right) trans = ((GameObject)Instantiate(trackPrefabs.smallRight, pos, Quaternion.identity)).transform;
                    else if(stob.subType == SpeedtrackObstacle.SpeedTrackType.Small_Up) trans = ((GameObject)Instantiate(trackPrefabs.smallUp, pos, Quaternion.identity)).transform;
                    else if(stob.subType == SpeedtrackObstacle.SpeedTrackType.Wide_Down) trans = ((GameObject)Instantiate(trackPrefabs.wideDown, pos, Quaternion.identity)).transform;
                    else if(stob.subType == SpeedtrackObstacle.SpeedTrackType.Wide_Left) trans = ((GameObject)Instantiate(trackPrefabs.wideLeft, pos, Quaternion.identity)).transform;
                    else if(stob.subType == SpeedtrackObstacle.SpeedTrackType.Wide_Right) trans = ((GameObject)Instantiate(trackPrefabs.wideRight, pos, Quaternion.identity)).transform;
                    else if(stob.subType == SpeedtrackObstacle.SpeedTrackType.Wide_Up) trans = ((GameObject)Instantiate(trackPrefabs.wideUp, pos, Quaternion.identity)).transform;

                    var estob = trans.gameObject.AddComponent<EditableSpeedTrackObstacle>();
                    estob.subType = stob.subType;
                    obstacles.Add(estob.obstacle);
                    break;
                default: break;
            }

            trans.position = pos;
            trans.localEulerAngles = rot;
            trans.localScale = scale;

            trans.parent = root;
        }
    }

    public void SaveLevel() {
        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        string serialized = JsonConvert.SerializeObject(obstacles, Formatting.Indented, settings);
        PlayerPrefs.SetString("Serialize Test", serialized);
        PlayerPrefs.Save();
        Debug.Log("Save Complete");
    }

    public void ClearPrefs() {
        PlayerPrefs.DeleteKey("Serialize Test");
        PlayerPrefs.Save();
        Debug.Log("All Prefs Cleared");
    }

    public void AddObstacle(Obstacle ob) {
        obstacles.Add(ob);
    }

    public void RemoveObstacle(Obstacle ob) {
        obstacles.Remove(ob);
    }
}

[System.Serializable]
public class CirlePrefabs {
    public GameObject circleSingle;
    public GameObject circleDouble;
    public GameObject circleTriple;
}

[System.Serializable]
public class GroundPrefabs {
    public GameObject ground;
    public GameObject fallingGround;
}

[System.Serializable]
public class SpeedTrackPrefabs {
    public GameObject smallDown;
    public GameObject smallLeft;
    public GameObject smallRight;
    public GameObject smallUp;
    public GameObject wideDown;
    public GameObject wideLeft;
    public GameObject wideRight;
    public GameObject wideUp;
}
