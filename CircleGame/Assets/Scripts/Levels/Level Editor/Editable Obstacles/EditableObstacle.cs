﻿using UnityEngine;
using System.Collections;

public class EditableObstacle : MonoBehaviour {
    private static tk2dCamera cam;
    private Transform trans;
    private SpriteRenderer[] sprites;
    private Color[] spriteColours;

    public static EditableObstacle currentObstacle { get; private set; }
    public readonly EditableProperties properties = new EditableProperties();
    public Obstacle obstacle { get; protected set; }
    public BoxCollider boundingBox { get; private set; }
    private bool shouldReposition;

    private const float leftPadding = 5f;
    private const float rightPadding = 5f;
    private const float upperPadding = 5f;
    private const float lowerPadding = 5f;

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

        LevelEditorManager.AddObstacle(obstacle);
    }

    private void OnScroll(float delta) { LevelEditorManager.instance.editorCam.Zoom(delta); }
    private void OnPress(bool pressed) {
        if(pressed) SetCurrentObject(this);

        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
            shouldReposition = pressed;

        if(!SOS.isMobile) {
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
            if(SOS.IsPointOnScreen(pos)) {
                Vector2 worldPos = (Vector2)Camera.main.ScreenToWorldPoint(pos);

                if(worldPos.x > (LevelEditorManager.worldBounds.size.x / 2f) - ((boundingBox.size.x * trans.localScale.x) / 2f))
                    worldPos.x = (LevelEditorManager.worldBounds.size.x / 2f) - ((boundingBox.size.x * trans.localScale.x) / 2f);
                else if(worldPos.x < -(LevelEditorManager.worldBounds.size.x / 2f) + ((boundingBox.size.x * trans.localScale.x) / 2f))
                    worldPos.x = -(LevelEditorManager.worldBounds.size.x / 2f) + ((boundingBox.size.x * trans.localScale.x) / 2f);

                if(worldPos.y > (LevelEditorManager.worldBounds.size.y / 2f) - ((boundingBox.size.y * trans.localScale.y) / 2f))
                    worldPos.y = (LevelEditorManager.worldBounds.size.y / 2f) - ((boundingBox.size.y * trans.localScale.y) / 2f);
                else if(worldPos.y < -(LevelEditorManager.worldBounds.size.y / 2f) + ((boundingBox.size.y * trans.localScale.y) / 2f))
                    worldPos.y = -(LevelEditorManager.worldBounds.size.y / 2f) + ((boundingBox.size.y * trans.localScale.y) / 2f);

                trans.position = worldPos;
                obstacle.position = trans.position;
            }
        }
    }

    public static void SetCurrentObject(EditableObstacle eob) {
        Vector3 center = Vector3.zero;
        if(currentObstacle != null) {
            center = new Vector3(currentObstacle.boundingBox.center.x, currentObstacle.boundingBox.center.y, 0);
            currentObstacle.boundingBox.center = center;
            for(int i = 0; i < currentObstacle.sprites.Length; i++)
                currentObstacle.sprites[i].color = currentObstacle.spriteColours[i];
        }

        if(eob != null) {
            center = new Vector3(eob.boundingBox.center.x, eob.boundingBox.center.y, -1);
            eob.boundingBox.center = center;
            for(int i = 0; i < eob.sprites.Length; i++)
                eob.sprites[i].color = LevelEditorManager.selectedObstacleColour;
        }
        currentObstacle = eob;
    }
}
