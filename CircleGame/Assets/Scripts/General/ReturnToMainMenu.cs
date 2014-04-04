using UnityEngine;
using System.Collections;

public class ReturnToMainMenu : MonoBehaviour {
	private void Update () {
        if(cInput.GetKeyUp("Escape"))
            Application.LoadLevel("Main Menu");
	}
}
