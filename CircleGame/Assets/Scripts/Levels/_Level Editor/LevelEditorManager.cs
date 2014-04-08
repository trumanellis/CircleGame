using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class LevelEditorManager : MonoBehaviour {
    private static LevelEditorManager instance;
    private Transform root;
    private List<Obstacle> obstacles = new List<Obstacle>();

    public LevelEditorCamera editorCam;
    public RadialMenu radialMenu;
    public GameObject circleSingle;
    public GameObject circleDouble;
    public GameObject circleTriple;

    private void Awake() {
        instance = this;
        root = transform;
    }

    private void Start() {
        if(PlayerPrefs.HasKey("Serialize Test")) {
            LoadLevel();
        }
    }

    public static void ShowRadialMenu(ObstacleType type) {
        instance.radialMenu.ShowRadialMenu(type);
    }

    public static void HideRadialMenu() {
        instance.radialMenu.HideRadialMenu();
    }

    private void OnScroll(float delta) {
        editorCam.Zoom(delta);
    }

    private void OnPress(bool pressed) {
        if(!SOS.isMobile) {
            if(!pressed && radialMenu.isShowing) radialMenu.HideRadialMenu();
            else if(pressed && Input.GetMouseButton(1)/* && !radialMenu.isShowing*/) radialMenu.ShowRadialMenu(ObstacleType.None);
        }
    }

    private void LoadLevel() {
        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        string map = PlayerPrefs.GetString("Serialize Test");
        obstacles = JsonConvert.DeserializeObject<List<Obstacle>>(map, settings);
        for(int i = 0; i < obstacles.Count; i++) {
            Vector3 pos = obstacles[i].position;
            Vector3 scale = obstacles[i].scale;
            Vector3 rot = obstacles[i].rotaion;
            Transform trans = null;

            switch(obstacles[i].obstacleType) {
                case ObstacleType.Circle:
                    CircleObstacle cob = obstacles[i] as CircleObstacle;
                    switch(cob.type) {
                        case CircleObstacle.CircleType.Circle_Single: break;
                        case CircleObstacle.CircleType.Circle_Double: break;
                        case CircleObstacle.CircleType.Circle_Triple:
                            trans = ((GameObject)Instantiate(circleTriple, pos, Quaternion.identity)).transform;
                            trans.localScale = scale;
                            trans.eulerAngles = rot;

                            trans.gameObject.AddComponent<BoxCollider>().size = new Vector3(4f, 4f, 0f);
                            EditableObstacle editob = trans.gameObject.AddComponent<EditableObstacle>();
                            editob.type = ObstacleType.Circle;
                            editob.obstacle = cob;
                            break;
                        default: break;
                    }
                    break;
                default: break;
            }

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
        PlayerPrefs.DeleteAll();
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
