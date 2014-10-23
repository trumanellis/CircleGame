using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class BuildAutomation : EditorWindow {
    private const string buildPathWithPatch = "";
    private const string buildPathNoPatch = "";
    private string fileName = PlayerSettings.productName;
    private bool windows;
    private bool mac;
    private bool linux;
    private bool createPatches;
    private bool windowsPatch;
    private bool macPatch;
    private bool linuxPatch;

    [MenuItem("Window/Build Automation")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(BuildAutomation));
    }

    private void OnGUI() {
        GUILayout.Label("Build Name", EditorStyles.boldLabel);
        fileName = EditorGUILayout.TextField("Build Name", fileName);

        GUILayout.Label("Platforms", EditorStyles.boldLabel);
        windows = EditorGUILayout.Toggle("PC", windows);
        mac = EditorGUILayout.Toggle("Mac", mac);
        linux = EditorGUILayout.Toggle("Linux", linux);


        if(windows || mac || linux) {
            createPatches = EditorGUILayout.BeginToggleGroup("Create Patches", createPatches);
            {
                if(windows) windowsPatch = EditorGUILayout.Toggle("Windows Patch", windowsPatch);
                if(mac) macPatch = EditorGUILayout.Toggle("Mac Patch", macPatch);
                if(linux) linuxPatch = EditorGUILayout.Toggle("Linux Patch", linuxPatch);
            }
            EditorGUILayout.EndToggleGroup();

            if(GUILayout.Button("Build")) BuildApp();
        }
    }

    private void BuildApp() {
        string[] scenes = GetSceneNames();

        if(windows) {
            Debug.Log("Building Windows");
            //if(EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows)
            //    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneWindows);
            if(windowsPatch && createPatches) {

            } else
                BuildPipeline.BuildPlayer(scenes, "Builds/PC/" + fileName + ".exe",
                                          BuildTarget.StandaloneWindows, BuildOptions.ShowBuiltPlayer);
        }
        if(mac) {
            Debug.Log("Building Mac");
            //if(EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneOSXUniversal)
            //    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneOSXUniversal);
            if(macPatch && createPatches) {

            } else 
                BuildPipeline.BuildPlayer(scenes, "Builds/Mac/" + fileName + ".app",
                                          BuildTarget.StandaloneOSXUniversal, BuildOptions.ShowBuiltPlayer);
        }
        if(linux) {
            Debug.Log("Building Linux");
            //if(EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneLinuxUniversal)
            //    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneLinuxUniversal);
            //if(linuxPatch && createPatches) {
            
            //} else 
            //    BuildPipeline.BuildPlayer(scenes, "Builds/Linux/" + fileName,
            //                              BuildTarget.StandaloneLinuxUniversal, BuildOptions.None);
        }
    }

    private void CreatePatch() {

    }

    private string[] GetSceneNames() {
        List<string> temp = new List<string>();
        foreach(UnityEditor.EditorBuildSettingsScene s in UnityEditor.EditorBuildSettings.scenes) {
            if(s.enabled)
                temp.Add(s.path);
        }
        return temp.ToArray();
    }
}
