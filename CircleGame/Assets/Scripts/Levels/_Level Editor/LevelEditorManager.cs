using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class LevelEditorManager : MonoBehaviour {
    private Transform root;
    private List<Obstacle> obstacles = new List<Obstacle>();

    public RadialMenu radialMenu;
    public GameObject circleSingle;
    public GameObject circleDouble;
    public GameObject circleTriple;

    private void Awake() {
        root = transform;
    }

    private void Start() {
        if(PlayerPrefs.HasKey("Serialize Test")) {
            Debug.Log("Start");
            LoadLevel();
        }
    }

    private void OnClick() {
        if(Input.GetMouseButtonUp(1) && !radialMenu.isShowing) radialMenu.ShowRadialMenu(ObstacleType.None);
        else if(Input.GetMouseButtonUp(0) && radialMenu.isShowing) radialMenu.HideRadialMenu();
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
