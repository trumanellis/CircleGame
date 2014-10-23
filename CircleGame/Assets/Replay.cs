using UnityEngine;
using System.Collections;

public class Replay : MonoBehaviour {
    public string levelToPlay = "Level for Lauren";

    private void OnClick() {
        Application.LoadLevel(levelToPlay);
    }
}
