using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class LevelEditorManager : MonoBehaviour {
    public static LevelEditorManager instance { get; private set; }
    public static GameObject currentGizmo { get; set; }

    private List<Obstacle> obstacles = new List<Obstacle>();
    public LevelEditorCamera editorCam;
    public Camera _uiCamera;
    public static Camera uiCamera;
    public RadialMenu radialMenu;

    public Color _selectedObstacleColour = Color.white;
    public static Color selectedObstacleColour;
    public Color _editableObstacleColour = Color.white;
    public static Color editableObstacleColour;
    public static BoxCollider worldBounds;
    public Transform obstaclesRoot;
    public CirlePrefabs circlePrefabs;
    public GroundPrefabs groundPrefabs;
    public SpeedTrackPrefabs trackPrefabs;
    public Transform playerStartMarker;

    private void Awake() {
        instance = this;
        uiCamera = _uiCamera;
        selectedObstacleColour = _selectedObstacleColour;
        editableObstacleColour = _editableObstacleColour;
        worldBounds = (BoxCollider)collider;
        if(IntroManager.mainMenuMusic != null && IntroManager.mainMenuMusic.isPlaying) IntroManager.mainMenuMusic.Stop();
    }

    private void Start() {
        if(PlayerPrefs.HasKey("Serialize Test")) {
            LoadLevel();
        }

        Vector3 pos = playerStartMarker.position;
        pos.z = -10;
        editorCam.transform.position = pos;
        editorCam.RepositionCamera();
    }

    public static void ShowRadialMenu(ObstacleType type) {
        instance.radialMenu.ShowRadialMenu(null);
    }

    public static void HideRadialMenu() {
        instance.radialMenu.HideRadialMenu();
    }

    private void OnScroll(float delta) {
        LevelEditorCamera.Zoom(delta);
    }

    private void OnPress(bool pressed) {
        if(!SOS.isMobile) {
            if(!pressed && Input.GetMouseButtonUp(1)) radialMenu.HideRadialMenu();
            else if(pressed && Input.GetMouseButtonDown(1)) {
                radialMenu.ShowRadialMenu(null);
                radialMenu.deleteButton.isEnabled = false;
            }
        }
    }

    private void OnClick() {
        if(Input.GetMouseButtonUp(0) && EditableObstacle.currentObstacle != null) EditableObstacle.currentObstacle.EditComplete();
    }

    private void LoadLevel() {
        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        string map = PlayerPrefs.GetString("Serialize Test");
        var obs = JsonConvert.DeserializeObject<List<Obstacle>>(map, settings);
        for(int i = 0; i < obs.Count; i++) {
            Vector3 pos = obs[i].position;
            Vector3 scale = obs[i].scale;
            Vector3 rot = obs[i].rotation;
            Transform trans = null;
            EditableObstacle ob = null;

            switch(obs[i].obstacleType) {
                case ObstacleType.Circle:
                    var cob = obs[i] as CircleObstacle;
                    if(cob.subType == CircleObstacle.CircleType.Circle_Single) trans = ((GameObject)Instantiate(circlePrefabs.circleSingle, pos, Quaternion.identity)).transform;
                    else if(cob.subType == CircleObstacle.CircleType.Circle_Double) trans = ((GameObject)Instantiate(circlePrefabs.circleDouble, pos, Quaternion.identity)).transform;
                    else trans = ((GameObject)Instantiate(circlePrefabs.circleTriple, pos, Quaternion.identity)).transform;

                    var ecob = trans.gameObject.AddComponent<EditableCircleObstacle>();
                    ecob.subType = cob.subType;
                    ecob.cob.showGround = cob.showGround;
                    ob = ecob;

                    if(!cob.showGround)
                        trans.Find("Ground").gameObject.SetActive(false);
                    break;
                case ObstacleType.Ground:
                    var gob = obs[i] as GroundObstacle;
                    if(gob.subType == GroundObstacle.GroundType.Ground) trans = ((GameObject)Instantiate(groundPrefabs.ground, pos, Quaternion.identity)).transform;
                    else if(gob.subType == GroundObstacle.GroundType.Falling_Ground) trans = ((GameObject)Instantiate(groundPrefabs.fallingGround, pos, Quaternion.identity)).transform;
                    else if(gob.subType == GroundObstacle.GroundType.Moving_Ground) trans = ((GameObject)Instantiate(groundPrefabs.movingGround, pos, Quaternion.identity)).transform;
                    else if(gob.subType == GroundObstacle.GroundType.Trampoline) trans = ((GameObject)Instantiate(groundPrefabs.trampoline, pos, Quaternion.identity)).transform;

                    var egob = trans.gameObject.AddComponent<EditableGroundObstacle>();
                    egob.subType = gob.subType;
                    ob = egob;
                    break;

                case ObstacleType.Speed_Track:
                    var stob = obs[i] as SpeedtrackObstacle;
                    if(stob.subType == SpeedtrackObstacle.SpeedTrackType.Small) trans = ((GameObject)Instantiate(trackPrefabs.small, pos, Quaternion.identity)).transform;
                    else if(stob.subType == SpeedtrackObstacle.SpeedTrackType.Wide) trans = ((GameObject)Instantiate(trackPrefabs.wide, pos, Quaternion.identity)).transform;
                    var estob = trans.gameObject.AddComponent<EditableSpeedTrackObstacle>();
                    estob.subType = stob.subType;
                    ob = estob;
                    break;
                case ObstacleType.Player_Start:
                    trans = playerStartMarker;
                    ob = trans.GetComponent<EditableObstacle>();
                    break;
                default: break;
            }
            if(trans != null) {
                trans.position = pos;
                trans.eulerAngles = rot;
                trans.localScale = scale;

                ob.obstacle.position = trans.position;
                ob.obstacle.rotation = trans.eulerAngles;
                ob.obstacle.scale = trans.localScale;

                if(trans != playerStartMarker) trans.parent = obstaclesRoot;
                else playerStartMarker.parent = transform;
            }
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
        obstacles.Clear();
        var children = new List<GameObject>();
        foreach(Transform child in obstaclesRoot) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
        obstacles.Add(playerStartMarker.GetComponent<EditableObstacle>().obstacle);
        if(currentGizmo != null) currentGizmo.SetActive(false);
    }

    public static void AddObstacle(Obstacle ob) {
        instance.obstacles.Add(ob);
    }

    public static void RemoveObstacle(Obstacle ob) {
        instance.obstacles.Remove(ob);
    }

    public void TestLevel() {
        if(obstacles.Count > 1) {
            SaveLevel();
            CustomLevelManager.fromEditor = true;
            Application.LoadLevel("Custom Level");
        }
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
    public GameObject movingGround;
    public GameObject trampoline;
}

[System.Serializable]
public class SpeedTrackPrefabs {
    public GameObject small;
    public GameObject wide;
}
