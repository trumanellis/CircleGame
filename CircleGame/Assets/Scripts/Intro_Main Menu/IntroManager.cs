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

    private Sound mainMenuMusic;

    private void Start() {
        mainMenuMusic = SoundManager.Play("Main Menu", true);
        showMenuTrigger.onButtonPress += ShowMainMenu;
        triggerArea.onAreaEnter += DetachTitle;
        cannon.onPlayerEnter += () => {
            player.moveController.trailEnabled = true;
            SOS.ExecuteMethod(menuStarter.cannonFireDelay, () => { cannon.shouldFire = true; });
        };
        openScreen.FadeOutScreen();
    }

    private void FadeComplete() {
        logoTween.TweenLogo();
    }

    private void LogoTweenComplete() {
        player.root.parent = null;
        player.body2D.gravityScale = 1f;
        player.col2D.enabled = true;
        playerCamera.SetTarget(player.trans);
        Destroy(logoTween.logo_player);
    }

    private void ShowMainMenu(EventTriggerButton button) {
        playerCamera.SetTarget(menuStarter.endFollowObject);
        player.trans.position = menuStarter.playerStartPosition.position;
        menuStarter.title.GetComponent<FollowTarget>().SetTarget(player.trans).shouldFollow = true;
        player.body2D.AddForce(Vector2.right * menuStarter.playerForce);
        triggerArea.enabled = true;
    }

    private void DetachTitle(EventTriggerArea area, GameObject obj) {
        menuStarter.title.GetComponent<FollowTarget>().shouldFollow = false;
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
    public float playerForce = 10f;
    public float cannonFireDelay = .8f;
}
