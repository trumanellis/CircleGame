﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CustomLevelManager : MonoBehaviour {
    public static LevelEditorManager instance { get; private set; }
    public static bool fromEditor { get; set; }
    private Transform root;

    public CameraFollow follow;
    public GameObject editorButton;
    public CirlePrefabs circlePrefabs;
    public GroundPrefabs groundPrefabs;
    public SpeedTrackPrefabs trackPrefabs;
    public Transform player;

    private void Start() {
        root = transform;
        if(PlayerPrefs.HasKey("Serialize Test"))
            LoadLevel();
        follow.SetTarget(player);
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

            switch(obs[i].obstacleType) {
                case ObstacleType.Circle:
                    var cob = obs[i] as CircleObstacle;
                    if(cob.subType == CircleObstacle.CircleType.Circle_Single) trans = ((GameObject)Instantiate(circlePrefabs.circleSingle, pos, Quaternion.identity)).transform;
                    else if(cob.subType == CircleObstacle.CircleType.Circle_Double) trans = ((GameObject)Instantiate(circlePrefabs.circleDouble, pos, Quaternion.identity)).transform;
                    else trans = ((GameObject)Instantiate(circlePrefabs.circleTriple, pos, Quaternion.identity)).transform;

                    if(!cob.showGround)
                        trans.Find("Ground").gameObject.SetActive(false);
                    break;
                case ObstacleType.Ground:
                    var gob = obs[i] as GroundObstacle;
                    if(gob.subType == GroundObstacle.GroundType.Ground) trans = ((GameObject)Instantiate(groundPrefabs.ground, pos, Quaternion.identity)).transform;
                    else if(gob.subType == GroundObstacle.GroundType.Falling_Ground) trans = ((GameObject)Instantiate(groundPrefabs.fallingGround, pos, Quaternion.identity)).transform;
                    else if(gob.subType == GroundObstacle.GroundType.Moving_Ground) trans = ((GameObject)Instantiate(groundPrefabs.movingGround, pos, Quaternion.identity)).transform;
                    else if(gob.subType == GroundObstacle.GroundType.Trampoline) trans = ((GameObject)Instantiate(groundPrefabs.trampoline, pos, Quaternion.identity)).transform;
                    break;
                case ObstacleType.Speed_Track:
                    var stob = obs[i] as SpeedtrackObstacle;
                    if(stob.subType == SpeedtrackObstacle.SpeedTrackType.Small) trans = ((GameObject)Instantiate(trackPrefabs.small, pos, Quaternion.identity)).transform;
                    else if(stob.subType == SpeedtrackObstacle.SpeedTrackType.Wide) trans = ((GameObject)Instantiate(trackPrefabs.wide, pos, Quaternion.identity)).transform;
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
