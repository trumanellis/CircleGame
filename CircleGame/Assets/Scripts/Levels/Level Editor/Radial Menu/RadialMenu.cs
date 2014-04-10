using UnityEngine;

public class RadialMenu : MonoBehaviour {
    private GameObject radialMenu;
    private Transform trans;
    private UITweener[] tweeners;
    private UIButton[] buttons;
    private ObstacleType currentType;
    private Vector2 location;

    public static RadialMenu instance { get; private set; }
    public RadialMenuButton[] menuButtons;
    public UIButton deleteButton;
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
    public EditablePlayerStartLocation playerStartLocation;

    private void Awake() {
        instance = this;
        trans = transform;
        radialMenu = gameObject;
        tweeners = GetComponentsInChildren<UITweener>();
        buttons = GetComponentsInChildren<UIButton>();
        for(int i = 0; i < tweeners.Length; i++)
            tweeners[i].duration = tweenDuration;
    }

    private void Start() { radialMenu.SetActive(false); }

    public void OnMenuHover(string name, bool enter) {
        menuLabel.text = enter ? name : (currentType == ObstacleType.None ? "General" : currentType.GetDescription());
    }

    public void OnMenuSelected(EditableProperties.Properties prop) {
        menuLabel.text = string.Empty;
        switch(prop) {
            case EditableProperties.Properties.Set_Player_Start: playerStartLocation.SetLocation(location); break;
        }
    }

    public void DeleteRequested() {
        if(EditableObstacle.currentObstacle != null) {
            LevelEditorManager.RemoveObstacle(EditableObstacle.currentObstacle.obstacle);
            Destroy(EditableObstacle.currentObstacle.gameObject);
        }
    }

    public void ShowRadialMenu(EditableObstacle ob) {
        if(!SOS.isMobile) location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var props = ob == null ? EditableProperties.blankEdits.GetProperties() : ob.properties.edits.GetProperties();
        int index = 0;
        for(; index < props.Length; index++) {
            menuButtons[index].gameObject.SetActive(true);
            menuButtons[index].property = props[index];
        }
        for(; index < menuButtons.Length; index++)
            menuButtons[index].gameObject.SetActive(false);

        currentType = ob == null ? ObstacleType.None : ob.obstacle.obstacleType;
        menuLabel.text = ob == null ? "General" : ob.obstacle.obstacleType.GetDescription();
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