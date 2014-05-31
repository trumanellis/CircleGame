using UnityEngine;
using System.Collections;

public class UpdateChecker : MonoBehaviour {
    public static UpdateData updateData;
    public bool checkOnAwake;
    private bool showUpdateWindow;

    private void Awake() {
        if(checkOnAwake) CheckForUpdate();
    }

    public static void CheckForUpdate() {
        updateData = Updater.CheckForUpdate(Ignis.version);

        if(updateData.updateFound) {
            Updater.DownloadReleaseNotes(() => {
                updateData.version = updateData.updater.getAvailableVersion();
                updateData.releaseNotes = updateData.updater.getReleaseNotes();
            });
        }
    }
}
