using UnityEngine;
using System.Collections;

public class DebugInfo : MonoBehaviour {
    private UILabel label;
    private UILabel fpsText;
    public string header = "Game Prototype";
    public float fpsUpdateFrequency = 0.5f;

    public int FramesPerSec { get; private set; }

    private void Awake() {
        label = GetComponent<UILabel>();
        if(!SOS.debugEnabled) { 
            enabled = false;
            label.enabled = false;
            return; }
        label.text = header + "\n" +
            "Version: " + SOS.version + "\n" +
            "Press " + SOS.quitKey.ToString() + " to exit\n" + 
            "FPS: " + FramesPerSec;

        StartCoroutine(FPS());
    }

    private IEnumerator FPS() {
        while(true) {
            // Capture frame-per-second
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(fpsUpdateFrequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            // Display it
            FramesPerSec = Mathf.RoundToInt(frameCount / timeSpan);
            label.text = header + "\n" +
                "Version: " + SOS.version + "\n" +
                (!SOS.isMobile ? "Press " + SOS.quitKey.ToString() + " to exit\n" : "") +
                "FPS: " + FramesPerSec;
        }
    }
}
