using UnityEngine;
using System.Collections;

public class EditableObstacle : MonoBehaviour {
    private static tk2dCamera cam;
    private SpriteRenderer[] sprites;
    private Color[] spriteColours;

    public readonly EditableProperties properties = new EditableProperties();
    public static EditableObstacle currentObstacle { get; private set; }
    public Transform trans { get; private set; }
    public BoxCollider boundingBox { get; private set; }
    public Obstacle obstacle { get; protected set; }

    private Vector2? mouseDelta;
    private bool shouldReposition;

    protected Vector2 maxScale = new Vector2(10f, 10f);
    protected Vector2 minScale = Vector2.one;
    protected bool uniformScale;

    private const float leftPadding = 5f;
    private const float rightPadding = 5f;
    private const float upperPadding = 5f;
    private const float lowerPadding = 5f;
    private const float positionFactor = .01f;
    private const float scaleFactor = .1f;
    private const float rotationFactor = .3f;

    protected virtual void Awake() {
        trans = transform;
        boundingBox = (BoxCollider)collider;

        if(cam == null)
            cam = Camera.main.GetComponent<tk2dCamera>();

        sprites = GetComponents<SpriteRenderer>();
        if(sprites.Length == 0)
            sprites = GetComponentsInChildren<SpriteRenderer>();

        spriteColours = new Color[sprites.Length];
        for(int i = 0; i < sprites.Length; i++)
            spriteColours[i] = sprites[i].color;

        SetSelectionDepth();
        LevelEditorManager.AddObstacle(obstacle);
    }

    private void OnScroll(float delta) { LevelEditorCamera.Zoom(delta); }
    private void OnPress(bool pressed) {
        if(pressed && currentObstacle != this) SetCurrentObject(this);

        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
            shouldReposition = pressed;

        if(!Ignis.isMobile) {
            if(!pressed) RadialMenu.instance.HideRadialMenu();
            else if(pressed && Input.GetMouseButtonDown(1)) {
                RadialMenu.instance.ShowRadialMenu(this);
                if(currentObstacle.obstacle.obstacleType == ObstacleType.Player_Start) RadialMenu.instance.deleteButton.isEnabled = false;
                else RadialMenu.instance.deleteButton.isEnabled = true;
            }
        }
    }

    private void Update() {
        if(shouldReposition) {
            Vector3 pos = Input.mousePosition;
            if(!mouseDelta.HasValue) {

            }
            if(Ignis.IsPointOnScreen(pos))
                Reposition((Vector2)Camera.main.ScreenToWorldPoint(pos));
        }
    }

    public string GetObstacleDescription() {
        string description = string.Empty;
        switch(obstacle.obstacleType) {
            case ObstacleType.Circle:
                description = ((CircleObstacle)obstacle).subType.GetDescription();
                break;
            case ObstacleType.Ground:
                description = ((GroundObstacle)obstacle).subType.GetDescription();
                break;
            case ObstacleType.Speed_Track:
                description = ((SpeedtrackObstacle)obstacle).subType.GetDescription();
                break;
            default: description = obstacle.obstacleType.GetDescription(); break;
        }
        return description;
    }

    private void Reposition(Vector2 pos) {
        if(pos.x > (LevelEditorManager.worldBounds.size.x / 2f) - ((boundingBox.size.x * Mathf.Abs(trans.localScale.x)) / 2f))
            pos.x = (LevelEditorManager.worldBounds.size.x / 2f) - ((boundingBox.size.x * Mathf.Abs(trans.localScale.x)) / 2f);
        else if(pos.x < -(LevelEditorManager.worldBounds.size.x / 2f) + ((boundingBox.size.x * Mathf.Abs(trans.localScale.x)) / 2f))
            pos.x = -(LevelEditorManager.worldBounds.size.x / 2f) + ((boundingBox.size.x * Mathf.Abs(trans.localScale.x)) / 2f);

        if(pos.y > (LevelEditorManager.worldBounds.size.y / 2f) - ((boundingBox.size.y * Mathf.Abs(trans.localScale.y)) / 2f))
            pos.y = (LevelEditorManager.worldBounds.size.y / 2f) - ((boundingBox.size.y * Mathf.Abs(trans.localScale.y)) / 2f);
        else if(pos.y < -(LevelEditorManager.worldBounds.size.y / 2f) + ((boundingBox.size.y * Mathf.Abs(trans.localScale.y)) / 2f))
            pos.y = -(LevelEditorManager.worldBounds.size.y / 2f) + ((boundingBox.size.y * Mathf.Abs(trans.localScale.y)) / 2f);

        trans.position = pos;
        obstacle.position = trans.position;
    }

    private void SetSelectionDepth() {
        //doing this will fake an alpha test. this will also need to be called every time we scale an obstacle
        float depth = -1 + (((boundingBox.size.x * trans.localScale.x) + (boundingBox.size.y * trans.localScale.y)) * .01f);
        Vector3 center = new Vector3(boundingBox.center.x, boundingBox.center.y, depth);
        boundingBox.center = center;
    }

    public static void SetCurrentObject(EditableObstacle eob) {
        if(LevelEditorManager.currentGizmo != null) LevelEditorManager.currentGizmo.SetActive(false);
        if(currentObstacle != null) {
            for(int i = 0; i < currentObstacle.sprites.Length; i++)
                currentObstacle.sprites[i].color = currentObstacle.spriteColours[i];
        }

        if(eob != null) {
            for(int i = 0; i < eob.sprites.Length; i++)
                eob.sprites[i].color = LevelEditorManager.selectedObstacleColour;
        }
        currentObstacle = eob;
    }

    public virtual void EditProperty(EditableProperties.Properties prop) {
        switch(prop) {
            case EditableProperties.Properties.Position: EditPosition(); break;
            case EditableProperties.Properties.Rotation: EditRotation(); break;
            case EditableProperties.Properties.Scale: EditScale(); break;
            default: break;
        }
    }

    //most common editable properties
    public virtual void EditPosition() {
        if(LevelEditorManager.currentGizmo != null) LevelEditorManager.currentGizmo.SetActive(false);
        CreateGizmo(EditPropertyUIController.positionGizmo.gameObject);
        EditPropertyUIController.positionGizmo.onGizmoDrag = (delta) => {
            trans.position += (Vector3)(delta * (positionFactor / cam.ZoomFactor));
            Reposition(trans.position);
            obstacle.position = trans.position;
        };

        for(int i = 0; i < sprites.Length; i++)
            sprites[i].color = LevelEditorManager.editableObstacleColour;
    }

    public virtual void EditRotation() {
        if(LevelEditorManager.currentGizmo != null) LevelEditorManager.currentGizmo.SetActive(false);
        CreateGizmo(EditPropertyUIController.rotationGizmo.gameObject);
        EditPropertyUIController.rotationGizmo.onGizmoDrag = (delta) => {
            trans.eulerAngles = new Vector3(trans.eulerAngles.x, trans.eulerAngles.y, trans.eulerAngles.z + (delta.y * (rotationFactor / cam.ZoomFactor)));
            obstacle.rotation = trans.eulerAngles;
        };

        for(int i = 0; i < sprites.Length; i++)
            sprites[i].color = LevelEditorManager.editableObstacleColour;
    }

    public virtual void EditScale() {
        if(LevelEditorManager.currentGizmo != null) LevelEditorManager.currentGizmo.SetActive(false);
        CreateGizmo(EditPropertyUIController.scaleGizmo.gameObject);
        EditPropertyUIController.scaleGizmo.onGizmoDrag = (delta) => {
            //need to check if it will become too large before setting it
            if(uniformScale) 
                SetScale(delta.x > 0 || delta.y > 0 ? Vector2.one : -Vector2.one);
            else
                SetScale(delta);
            Reposition(trans.position);
            obstacle.scale = trans.localScale;
            SetSelectionDepth();
        };
        EditPropertyUIController.scaleGizmo.transform.eulerAngles = trans.eulerAngles;

        for(int i = 0; i < sprites.Length; i++)
            sprites[i].color = LevelEditorManager.editableObstacleColour;
    }

    private void SetScale(Vector3 delta) {
        delta = (Vector3)(delta * (scaleFactor / cam.ZoomFactor));

        Vector3 scale = trans.localScale + delta;
        scale.x = Mathf.Clamp(scale.x, minScale.x, maxScale.x);
        scale.y = Mathf.Clamp(scale.y, minScale.y, maxScale.y);

        trans.localScale = scale;
    }

    private void CreateGizmo(GameObject gizmo) {
        gizmo.SetActive(true);
        gizmo.transform.position = LevelEditorManager.uiCamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(trans.position));
        gizmo.GetComponent<GizmoFollow>().obstacle = trans;
        LevelEditorManager.currentGizmo = gizmo;
    }

    public void EditComplete() {
        if(LevelEditorManager.currentGizmo != null) LevelEditorManager.currentGizmo.SetActive(false);
        for(int i = 0; i < sprites.Length; i++)
            sprites[i].color = spriteColours[i];
        currentObstacle = null;
    }
}
