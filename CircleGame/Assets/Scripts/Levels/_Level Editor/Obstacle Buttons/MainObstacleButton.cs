using UnityEngine;
using System.Collections;

public class MainObstacleButton : MonoBehaviour {
    public GameObject subMenu;
    private static GameObject currentlyShownMenu;

    private void OnClick() {
        if(currentlyShownMenu != null && currentlyShownMenu != subMenu)
            HideSubMenu();
        else currentlyShownMenu = subMenu;

        subMenu.SetActive(true);
        subMenu.GetComponent<TweenPosition>().PlayForward();


    }

    public void HideSubMenu() {
        UIScrollView sv = currentlyShownMenu.GetComponent<UIScrollView>();
        if(sv != null) sv.ResetPosition();
        TweenPosition tweener = currentlyShownMenu.GetComponent<TweenPosition>();

        tweener.onFinished.Add(new EventDelegate(() => {
            currentlyShownMenu.SetActive(false);
            currentlyShownMenu = subMenu;
        }) {
            oneShot = true       
        });

        tweener.PlayReverse();
    }
}
