using UnityEngine;
using System.Collections;

public class UpdateChecker : MonoBehaviour {
    private UpdateData data;
    private bool showUpdateWindow;

    private void Awake() {
        UpdateData data = Updater.CheckForUpdate(SOS.version);

        if(data.updateFound) {
            Updater.DownloadReleaseNotes(() => {
                showUpdateWindow = true;
            });
        }
    }

    private void Update() {
        if(showUpdateWindow) {
            showUpdateWindow = false;
            //transform.Find<UpdateWindow>().ShowWindow(true);
            enabled = false;
        }
    }
}
