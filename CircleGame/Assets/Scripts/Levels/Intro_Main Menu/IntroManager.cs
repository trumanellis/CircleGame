using UnityEngine;
using System.Collections;

public class IntroManager : MonoBehaviour {
    public CameraFollow playerCamera;
    public Player player;
    public EventTriggerButton showMenuTrigger;
    public EventTriggerArea triggerArea;
    public Absorb_FirePlayer cannon;
    public OpeningScreen openScreen;
    public IntroLogoTween logoTween;
    public MainMenuStarter menuStarter;

    private static Sound mainMenuMusic;

    private void Start() {
        if(mainMenuMusic == null) mainMenuMusic = SoundManager.Play("Main Menu").Loop(true);

        showMenuTrigger.onButtonPress += ShowMainMenu;
        triggerArea.onAreaEnter += DetachTitle;
        cannon.onPlayerEnter += () => {
            player.moveController.trailEnabled = true;
            SOS.ExecuteMethod(menuStarter.cannonFireDelay, () => {
                cannon.shouldFire = true;
                menuStarter.ShowButtons();
            });
        };
        openScreen.FadeOutScreen();
    }

    private void FadeComplete() {
        logoTween.TweenLogo();
        StartCoroutine("CheckForSkipRequest");
    }

    private void LogoTweenComplete() {
        EnablePlayer();
        playerCamera.SetTarget(player.trans);
        Destroy(logoTween.logo_player);
    }

    private void ShowMainMenu(EventTriggerButton button) {
        StopCoroutine("CheckForSkipRequest");
        Destroy(showMenuTrigger.gameObject);
        playerCamera.SetTarget(menuStarter.endFollowObject);
        player.trans.position = menuStarter.playerStartPosition.position;
        menuStarter.title.GetComponent<FollowTarget>().SetTarget(player.trans).shouldFollow = true;
        player.body2D.velocity = Vector2.right * menuStarter.playerSpeed;
        triggerArea.enabled = true;
    }

    private void DetachTitle(EventTriggerArea area, GameObject obj) {
        menuStarter.title.GetComponent<FollowTarget>().shouldFollow = false;
    }

    private void EnablePlayer() {
        player.root.parent = null;
        player.body2D.gravityScale = 1f;
        player.col2D.enabled = true;
    }

    private void SkipIntro() {
        iTween.Stop();
        EnablePlayer();
        ShowMainMenu(null);
    }

    private IEnumerator CheckForSkipRequest() {
        while(true) {
            if(cInput.GetVirtualKeyUp("Escape")) {
                SkipIntro();
                yield break;
            } yield return 1;
        }
    }
}

[System.Serializable]
public class OpeningScreen {
    public GameObject startScreen;
    public GameObject forCallbacks;
    public Color fadeColour;
    public float fadeDuration = .5f;
    public iTween.EaseType easeTtpye;

    public void FadeOutScreen() {
        if(!startScreen.activeSelf) startScreen.SetActive(true);
        iTween.ColorTo(startScreen, iTween.Hash(
            "color", fadeColour,
            "time", fadeDuration,
            "easetype", easeTtpye,
            "oncomplete", "FadeComplete",
            "oncompletetarget", forCallbacks
            ));
    }
}

[System.Serializable]
public class IntroLogoTween {
    public GameObject logo_player;
    public GameObject forCallbacks;
    public float tweenDuration = .5f;
    public float delay = 0f;
    public iTween.EaseType easeTtpye;

    public void TweenLogo() {
        iTween.MoveTo(logo_player, iTween.Hash(
            "x", 9,
            "delay", delay,
            "time", tweenDuration,
            "easetype", easeTtpye,
            "oncomplete", "LogoTweenComplete",
            "oncompletetarget", forCallbacks
            ));
    }
}

[System.Serializable]
public class MainMenuStarter {
    public Transform endFollowObject;
    public Transform title;
    public Transform playerStartPosition;
    public float playerSpeed = 50f;
    public float cannonFireDelay = .8f;
    public UITweener updateButton;
    public UITweener[] buttons;

    public void ShowButtons() {
        for(int i = 0; i < buttons.Length; i++) {
            UIButton button = buttons[i].GetComponent<UIButton>();
            button.isEnabled = false;
            buttons[i].onFinished.Add(new EventDelegate(() => { button.isEnabled = true; }));
            buttons[i].PlayForward();
        }

        if(UpdateChecker.updateData != null) {
            if(UpdateChecker.updateData.updateFound) {
                updateButton.gameObject.SetActive(true);
                UIButton ubutton = updateButton.GetComponent<UIButton>();
                ubutton.isEnabled = false;
                updateButton.onFinished.Add(new EventDelegate(() => { ubutton.isEnabled = true; }));
            }
        }
    }
}
