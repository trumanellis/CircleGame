using UnityEngine;
using System;
using System.Collections;

public class DebugConsole : MonoBehaviour {
    //colours used for log status
    private const string norm = "[FFFFFF]";
    private const string warn = "[FFFF70]";
    private const string error = "[FF5151]";

    private static DebugConsole instance;
    private UIInput inputField;
    private bool initialized;

    public static UITextList textList { get; set; }
    public bool startShowing;
    public bool isShowing { get; private set; }
    public KeyCode displayKey = KeyCode.F9;

    public DebugConsole Init() {
        initialized = true;

        instance = this;
        inputField = transform.Find<UIInput>();
        textList = transform.Find<UITextList>();
        return this;
    }

    public void Show(bool show) {
        if(!initialized) Init();
        isShowing = show;
        gameObject.SetActive(show);
        inputField.isSelected = show;
    }

    public void OnSubmit() {
        if(inputField.value.StartsWith("/")) SOS.ExecuteCommand(inputField.value.Remove(0, 1));
        else Debug.Log("(Player) " + inputField.value);

        inputField.value = string.Empty;
        inputField.isSelected = true;
    }

    public void HandleLogEntry(string logEntry, string stackTrace, LogType logType) {
        switch(logType) {
            case LogType.Warning:
                LogWarning(logEntry);
                break;
            case LogType.Exception:
            case LogType.Error:
                LogError(logEntry + "\n" + CreateReadableStackTrace(stackTrace));
                break;
            default:
                Log(logEntry);
                break;
        }
    }

    private string CreateReadableStackTrace(string stackTrace) {
        return stackTrace.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)[0];
    }

    private static void PrintToConsole(string str, LogStatus status) {
        //handle to actual printing
        string text = NGUIText.StripSymbols(str);
        if(!string.IsNullOrEmpty(text)) {
            string timeStamp = DateTime.Now.ToString("HH:mm:ss tt");
            switch(status) {
                case LogStatus.Normal:
                    textList.Add("[FFFFFF]" + timeStamp + "[-]" + " - " + norm + text + "[-]");
                    break;
                case LogStatus.Warning:
                    textList.Add("[FFFFFF]" + timeStamp + "[-]" + " - " + warn + "[Warning] " + text + "[-]");
                    break;
                case LogStatus.Error:
                    textList.Add("[FFFFFF]" + timeStamp + "[-]" + " - " + error + "[Error] " + text + "[-]");
                    break;
            }
        }
    }

    public static void Log(object obj) {
        PrintToConsole(obj.ToString(), LogStatus.Normal);
    }

    public static void LogWarning(object obj) {
        PrintToConsole(obj.ToString(), LogStatus.Warning);
    }

    public static void LogError(object obj) {
        LogError(obj, false);
    }

    public static void LogError(object obj, bool pause) {
        PrintToConsole(obj.ToString(), LogStatus.Error);
        instance.isShowing = true;
        DebugConsole.instance.Show(instance.isShowing);
        if(pause) Debug.Break();
    }

    public enum LogStatus { Normal, Warning, Error }
}
