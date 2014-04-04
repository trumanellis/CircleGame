using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {
    public UpdateNotification updateButton;
    private bool updating;
    private bool pendingRestart;
    private bool pendingCancel;
    private bool finishUpdate;

    public void PlayClicked() {
        Debug.Log("Play clicked");
        Application.LoadLevel("Dev Level");
    }

    public void StartNewGameClicked() {
        Debug.Log("Start New Game clicked");
        Application.LoadLevel("Level Editor");
    }

    public void SelectStoryClicked() {
        Debug.Log("Select Story clicked");
    }

    public void UpdateButtonClicked() {
        if(pendingRestart) Application.Quit();
        updateButton.button.isEnabled = false;
        updateButton.tweener.PlayReverse();
        updateButton.tweener.onFinished.Add(new EventDelegate(() => {
            updateButton.button.isEnabled = true;
            updateButton.nameLabel.gameObject.SetActive(false);
            updateButton.closeX.SetActive(false);
            updateButton.cancelLabel.SetActive(true);
            updateButton.progressBar.gameObject.SetActive(true);
            pendingRestart = true;
            updating = true;
            Updater.onProgress = OnProgressUpdate;
            Updater.Download(() => { if(!pendingCancel) Updater.ExtractPatch(() => { finishUpdate = true; }); });
            updateButton.tweener.PlayForward();
        }));
    }

    public void OnProgressUpdate(double progress) {
        updateButton.progressBar.value = (float) progress;
    }

    public void UpdateButtonCancelled() {
        updateButton.button.isEnabled = false;
        updateButton.tweener.PlayReverse();
        if(updating) pendingCancel = true;
    }

    private void Update() { if(finishUpdate) FinishUpdate(); }
    private void FinishUpdate() {
        updateButton.cancelLabel.SetActive(false);
        updateButton.progressBar.gameObject.SetActive(false);
        updateButton.nameLabel.text = "Click To Close";
        updateButton.button.isEnabled = true;
        pendingRestart = true;
    }
}
[System.Serializable]
public class UpdateNotification {
    public GameObject buttonObject;
    public UIButton button;
    public UITweener tweener;
    public UISlider progressBar;
    public UILabel nameLabel;
    public GameObject closeX;
    public GameObject cancelLabel;
}
