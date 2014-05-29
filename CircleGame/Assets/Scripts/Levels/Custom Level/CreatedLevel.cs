using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class CreatedLevel : MonoBehaviour {
    public static string path;
    public static bool dirCreated;

    public static bool CreateLevelDirectory() {
        if(string.IsNullOrEmpty(path))
            path = Application.dataPath + @"/Custom Levels";
        try {
            if(!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            dirCreated = true;
        } catch(Exception e) {
            Debug.LogError(e.ToString());
            return false;
        } return true;
    }

    public static bool SaveLevel(string levelName, string levelData) {
        if(!dirCreated) {
            if(CreateLevelDirectory()) {
                return _SaveLevel(levelName, levelData);
            } else return false;
        } return _SaveLevel(levelName, levelData);
    }

    private static bool _SaveLevel(string levelName, string levelData) {
        try {
            File.WriteAllText(path + "/" + levelName + ".level", levelData);
            //File.SetAttributes(path + "/" + levelName + ".level", FileAttributes.ReadOnly);
        } catch(Exception e) {
            Debug.LogError(e.ToString());
            return false;
        } return true;
    }

    public static string LoadLevel(string levelName) {
        if(string.IsNullOrEmpty(path))
            path = Application.dataPath + @"/Custom Levels";
        try {
            if(Directory.Exists(path)) {
                string levelData = "";
                using(StreamReader sr = new StreamReader(path + "/" + levelName + ".level")) {
                    levelData = sr.ReadToEnd();
                } return levelData;
            } return null;
        } catch(Exception e) {
            Debug.LogError(e);
            return null;
        }
    }
}
