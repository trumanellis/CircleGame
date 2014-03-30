using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {
    public UpdateNotification updateButton;
    private bool updating;
    private bool pendingRestart;
    private bool pendingCancel;

    public void PlayClicked() {
        Debug.Log("Play clicked");
        Application.LoadLevel("Level1");
    }

    public void StartNewGameClicked() {
        Debug.Log("Start New Game clicked");
    }

    public void SelectStoryClicked() {
        Debug.Log("Select Story clicked");
    }

    public void UpdateButtonClicked() {
        if(pendingRestart) Application.Quit();
        Debug.Log("Update Button Clicked");
        updateButton.button.isEnabled = false;
        updateButton.tweener.PlayReverse();
        updateButton.tweener.onFinished.Add(new EventDelegate(() => {
            updateButton.button.isEnabled = true;
            //updateButton.nameLabel.gameObject.SetActive(false);
            updateButton.closeX.SetActive(false);
            updateButton.cancelLabel.SetActive(true);
            //updateButton.progressBar.gameObject.SetActive(true);
            updateButton.nameLabel.text = "Click To Close";
            pendingRestart = true;
            updating = true;

            //Updater.Download(() => {
            //    if(!Application.isEditor && !pendingCancel)
            //        Updater.ExtractPatch(() => {
            //            updateButton.cancelLabel.SetActive(false);
            //            updateButton.progressBar.gameObject.SetActive(false);
            //            updateButton.nameLabel.text = "Click To Close";
            //            updateButton.button.isEnabled = true;
            //            pendingRestart = true;
            //        });
            //});
            Updater.onProgress = OnProgressUpdate;
            updateButton.tweener.PlayForward();
        }));
    }

    public void OnProgressUpdate(double progress) {
        //updateButton.progressBar.value = (float) progress;
    }

    public void UpdateButtonCancelled() {
        updateButton.button.isEnabled = false;
        updateButton.tweener.PlayReverse();
        if(updating) pendingCancel = true;
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
