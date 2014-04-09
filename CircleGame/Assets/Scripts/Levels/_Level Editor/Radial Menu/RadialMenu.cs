using UnityEngine;

public class RadialMenu : MonoBehaviour {
    private GameObject radialMenu;
    private Transform trans;
    private UITweener[] tweeners;
    private UIButton[] buttons;
    private ObstacleType currentType;

    public Camera uiCamera;
    public Vector2 size;
    public bool isShowing { get; set; }
    public float tweenDuration = .1f;
    public float radialRightPadding = 5f;
    public float radialLeftPadding = 5f;
    public float radialUpperPadding = 5f;
    public float radialLowerPadding = 5f;
    public GameObject innerCircle;
    public UILabel menuLabel;

    private void Awake() {
        trans = transform;
        radialMenu = gameObject;
        tweeners = GetComponentsInChildren<UITweener>();
        buttons = GetComponentsInChildren<UIButton>();
        for(int i = 0; i < tweeners.Length; i++)
            tweeners[i].duration = tweenDuration;
        for(int i = 0; i < buttons.Length; i++)
            buttons[i].isEnabled = false;
        radialMenu.SetActive(false);
    }

    public void OnMenuHover(string name, bool enter) {
        menuLabel.text = enter ? name : (currentType == ObstacleType.None ? "General" : currentType.ToString());
    }

    public void OnMenuSelected(string name) {
        menuLabel.text = string.Empty;
        Debug.Log("Selected " + name);
    }

    public void ShowRadialMenu(ObstacleType type) {
        //show code
        currentType = type;
        menuLabel.text = type == ObstacleType.None ? "General" : type.ToString();
        radialMenu.SetActive(true);
        Vector3 radialPos = Input.mousePosition;
        if(radialPos.x >= Screen.width / 2f && radialPos.x > Screen.width - size.x / 2f - radialRightPadding) radialPos.x = Screen.width - (size.x / 2f) - radialRightPadding;
        else if(radialPos.x < Screen.width / 2f && radialPos.x < size.x / 2f + radialLeftPadding) radialPos.x = size.x / 2f + radialLeftPadding;

        if(radialPos.y >= Screen.height / 2f && radialPos.y > Screen.height - size.y / 2f - radialUpperPadding) radialPos.y = Screen.height - (size.y / 2f) - radialUpperPadding;
        else if(radialPos.y < Screen.height / 2f && radialPos.y < size.y / 2f + radialLowerPadding) radialPos.y = size.y / 2f + radialLowerPadding;

        radialPos.z = 0;
        trans.position = uiCamera.ScreenToWorldPoint(radialPos);
        for(int i = 0; i < tweeners.Length; i++)
            tweeners[i].PlayForward();
        for(int i = 0; i < buttons.Length; i++)
            buttons[i].isEnabled = true;
        isShowing = true;
    }

    public void HideRadialMenu() {
        menuLabel.text = string.Empty;
        for(int i = 0; i < tweeners.Length; i++)
            tweeners[i].PlayReverse();
        for(int i = 0; i < buttons.Length; i++)
            buttons[i].isEnabled = false;
    }
}