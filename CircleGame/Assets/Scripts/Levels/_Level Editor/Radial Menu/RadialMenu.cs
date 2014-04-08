using UnityEngine;

public class RadialMenu : MonoBehaviour {
    private GameObject radialMenu;
    private Transform trans;

    public Camera uiCamera;
    public Vector2 size;
    public UITweener[] tweeners;
    public bool isShowing { get; set; }
    public float tweenDuration = .1f;
    public float radialRightPadding = 5f;
    public float radialLeftPadding = 5f;
    public float radialUpperPadding = 5f;
    public float radialLowerPadding = 5f;

    private void Awake() {
        trans = transform;
        radialMenu = gameObject;
        tweeners = GetComponentsInChildren<UITweener>();
        for(int i = 0; i < tweeners.Length; i++)
            tweeners[i].duration = tweenDuration;
        radialMenu.SetActive(false);
    }

    public void ShowRadialMenu(ObstacleType type) {
        //show code
        radialMenu.SetActive(true);
        Vector3 radialPos = Input.mousePosition;
        if(radialPos.x >= Screen.width / 2f && radialPos.x > Screen.width - size.x / 2f) radialPos.x = Screen.width - (size.x / 2f) - radialRightPadding;
        else if(radialPos.x < Screen.width / 2f && radialPos.x < size.x / 2f) radialPos.x = size.x / 2f + radialLeftPadding;

        if(radialPos.y >= Screen.height / 2f && radialPos.y > Screen.height - size.y / 2f) radialPos.y = Screen.height - (size.y / 2f) - radialUpperPadding;
        else if(radialPos.y < Screen.height / 2f && radialPos.y < size.y / 2f) radialPos.y = size.y / 2f + radialLowerPadding;

        radialPos.z = 0;
        trans.position = uiCamera.ScreenToWorldPoint(radialPos);
        for(int i = 0; i < tweeners.Length; i++)
            tweeners[i].PlayForward();
        isShowing = true;
    }

    public void HideRadialMenu() {
        for(int i = 0; i < tweeners.Length; i++)
            tweeners[i].PlayReverse();
        //SOS.ExecuteMethod(tweenDuration, () => { radialMenu.SetActive(false); isShowing = false; });
    }

    //private void OnDrawGizmos() {
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, .1f));
    //}
}