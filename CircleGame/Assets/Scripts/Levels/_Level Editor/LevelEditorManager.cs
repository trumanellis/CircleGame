using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class LevelEditorManager : MonoBehaviour {
    private Transform root;
    private List<Obstacle> obstacles = new List<Obstacle>();

    private void Awake() {
        root = transform;
    }

    private void Start() {
        if(PlayerPrefs.HasKey("Serialize Test")) {
            LoadLevel();
        }
    }

    private void Update() {
        if(Input.GetKeyUp(KeyCode.S)) {
            SaveLevel();
        } else if(Input.GetKeyUp(KeyCode.Q)) {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }

    private void LoadLevel() {
        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        string map = PlayerPrefs.GetString("Serialize Test");
        Debug.Log(PlayerPrefs.GetString("Serialize Test"));
        obstacles = JsonConvert.DeserializeObject<List<Obstacle>>(map, settings);
        for(int i = 0; i < obstacles.Count; i++) {
            Vector3 pos = obstacles[i].position;

        }
    }

    private void SaveLevel() {
        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        string serialized = JsonConvert.SerializeObject(obstacles, Formatting.Indented, settings);
        PlayerPrefs.SetString("Serialize Test", serialized);
        PlayerPrefs.Save();
        Debug.Log("Save Complete");
    }

    public void AddObstacle(Obstacle ob) {
        obstacles.Add(ob);
    }
}
