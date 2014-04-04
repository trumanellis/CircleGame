using UnityEngine;
using System.Collections;

public class OnScreenControlsManager : MonoBehaviour {
    private void Awake() {
        if(!SOS.isMobile) gameObject.SetActive(false);
    }

    private void Update() {
        if(cInput.GetVirtualKeyDown("Left")) Debug.Log("Left Was pressed");
        else if(cInput.GetVirtualKeyUp("Left")) Debug.Log("Left Was Released");
        if(cInput.GetVirtualKeyDown("Right")) Debug.Log("Right Was pressed");
        else if(cInput.GetVirtualKeyUp("Right")) Debug.Log("Right Was Released");

        if(cInput.GetVirtualKeyDown("Jump")) Debug.Log("Jump Was pressed");
        else if(cInput.GetVirtualKeyUp("Jump")) Debug.Log("Jump Was Released");
    }
}
