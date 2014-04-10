using UnityEngine;
using System.Collections;

public class MainObstacleButton : MonoBehaviour {
    public GameObject subMenu;
    private static GameObject currentlyShownMenu;

    private void OnClick() {
        if(currentlyShownMenu != null && currentlyShownMenu == subMenu) ReplaceSubMenuWith(null);
        else if(currentlyShownMenu != null) ReplaceSubMenuWith(subMenu);
        else ShowSubMenu();
    }

    private void ShowSubMenu() {
        subMenu.SetActive(true);
        var tweener = subMenu.GetComponent<TweenPosition>();
        tweener.onFinished.Clear();
        tweener.PlayForward();
        currentlyShownMenu = subMenu;
    }

    private void ReplaceSubMenuWith(GameObject replacement) {
        var sv = currentlyShownMenu.GetComponent<UIScrollView>();
        if(sv != null) sv.ResetPosition();

        var tweener = currentlyShownMenu.GetComponent<TweenPosition>();
        tweener.onFinished.Add(new EventDelegate(() => {
            tweener.gameObject.SetActive(false);
        }) {
            oneShot = true
        });
        tweener.PlayReverse();
        currentlyShownMenu = replacement;
        if(replacement != null) ShowSubMenu();
    }
}
