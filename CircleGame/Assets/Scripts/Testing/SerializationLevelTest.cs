using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class SerializationLevelTest : MonoBehaviour {
    public List<GameObject> gos;
    private List<Obstacle> obstacles = new List<Obstacle>();

    private void Start() {
        if(PlayerPrefs.HasKey("Serialize Test")) {
            LoadLevel();
        }
    }

    private void Update() {
        if(cInput.GetButtonUp("Left Mouse")) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            GameObject go = Instantiate(gos[UnityEngine.Random.Range(0, gos.Count)], pos, Quaternion.identity) as GameObject;
            Transform trans = go.transform;
            Obstacle ob = new Obstacle();
            ob.position = trans.position;
            ob.rotaion = trans.rotation.eulerAngles;
            ob.scale = trans.localScale;
            ob.obstacleType = ObstacleType.Ground;
            obstacles.Add(ob);
        } else if(Input.GetKeyUp(KeyCode.S)) {
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
            Instantiate(gos[UnityEngine.Random.Range(0, gos.Count)], pos, Quaternion.identity);
        }
    }

    private void SaveLevel() {
        JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        string serialized = JsonConvert.SerializeObject(obstacles, Formatting.Indented, settings);
        PlayerPrefs.SetString("Serialize Test", serialized);
        PlayerPrefs.Save();
        Debug.Log("Save Complete");
    }
}


