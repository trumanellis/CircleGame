using UnityEngine;
using System.Collections;

public class EditableObstacle : MonoBehaviour {
    private static tk2dCamera cam;
    private Transform trans;
    private SpriteRenderer[] sprites;
    private Color[] spriteColours;
    private iTween.EaseType ease = iTween.EaseType.easeInExpo;

    public static EditableObstacle currentObstacle { get; private set; }
    public readonly EditableProperties properties = new EditableProperties();
    public Obstacle obstacle { get; protected set; }
    public BoxCollider boundingBox { get; private set; }
    private float heldTime;
    private float showTime = .5f;
    private bool shouldCount;
    private bool shouldReposition;
    private bool showingMenu;

    private const float leftPadding = 5f;
    private const float rightPadding = 5f;
    private const float upperPadding = 5f;
    private const float lowerPadding = 5f;

    protected virtual void Awake() {
        trans = transform;
        boundingBox = (BoxCollider)collider;
        if(cam == null) cam = Camera.main.GetComponent<tk2dCamera>();
        sprites = GetComponents<SpriteRenderer>();
        if(sprites.Length == 0) sprites = GetComponentsInChildren<SpriteRenderer>();
        spriteColours = new Color[sprites.Length];
        for(int i = 0; i < sprites.Length; i++)
            spriteColours[i] = sprites[i].color;
    }

    private void OnScroll(float delta) { LevelEditorManager.instance.editorCam.Zoom(delta); }
    private void OnPress(bool pressed) {
        if(pressed) SetCurrentObject(this);

        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
            shouldReposition = pressed;

        if(!SOS.isMobile) {
            if(!pressed) RadialMenu.instance.HideRadialMenu();
            else if(pressed && Input.GetMouseButtonDown(1)) {
                RadialMenu.instance.deleteButton.isEnabled = true;
                RadialMenu.instance.ShowRadialMenu(this);
            }
        }
    }

    private void Update() {
        if(shouldReposition && !showingMenu) {
            Vector3 pos = Input.mousePosition;
            if(pos.x > Screen.width - (boundingBox.size.x / 2f) * (100f * cam.ZoomFactor) - leftPadding) pos.x = Screen.width - (boundingBox.size.x / 2f) * (100f * cam.ZoomFactor) - leftPadding;
            else if(pos.x < (boundingBox.size.x / 2f) * (100f * cam.ZoomFactor) + rightPadding) pos.x = (boundingBox.size.x / 2f) * (100f * cam.ZoomFactor) + rightPadding;
            if(pos.y > Screen.height - (boundingBox.size.y / 2f) * (100f * cam.ZoomFactor) - upperPadding) pos.y = Screen.height - (boundingBox.size.y / 2f) * (100f * cam.ZoomFactor) - upperPadding;
            else if(pos.y < (boundingBox.size.y / 2f) * (100f * cam.ZoomFactor) + lowerPadding) pos.y = (boundingBox.size.y / 2f) * (100f * cam.ZoomFactor) + lowerPadding;

            trans.position = (Vector2)Camera.main.ScreenToWorldPoint(pos);
            obstacle.position = trans.position;
        }
    }

    private void StopScaling() {
        heldTime = 0f;
        shouldReposition = false;
        shouldCount = false;
    }

    public static void SetCurrentObject(EditableObstacle eob) {
        Vector3 center = new Vector3(eob.boundingBox.center.x, eob.boundingBox.center.y, 0);
        if(currentObstacle != null) {
            currentObstacle.boundingBox.center = center;
            for(int i = 0; i < currentObstacle.sprites.Length; i++)
                currentObstacle.sprites[i].color = currentObstacle.spriteColours[i];
        }
        for(int i = 0; i < eob.sprites.Length; i++)
            eob.sprites[i].color = LevelEditorManager.selectedObstacleColour;
        center.z = -1;
        eob.boundingBox.center = center;
        currentObstacle = eob;
    }
}
