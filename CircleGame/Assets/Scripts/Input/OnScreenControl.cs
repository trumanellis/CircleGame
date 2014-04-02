using UnityEngine;
using System.Collections;

public class OnScreenControl : MonoBehaviour {
    public enum KeyType { Left, Right, Jump, Escape }
    public KeyType keyType = KeyType.Left;

    private void OnDragOver(GameObject obj) {
        switch(keyType) {
            case KeyType.Left: cInput.PressVirtualKey("Left"); break;
            case KeyType.Right: cInput.PressVirtualKey("Right"); break;
        }
    }

    private void OnDragOut(GameObject obj) {
        switch(keyType) {
            case KeyType.Left: cInput.ReleaseVirtualKey("Left"); break;
            case KeyType.Right: cInput.ReleaseVirtualKey("Right"); break;
        }
    }

    private void OnPress(bool press) {
        if(press) {
            switch(keyType) {
                case KeyType.Left: cInput.PressVirtualKey("Left"); break;
                case KeyType.Right: cInput.PressVirtualKey("Right"); break;
                case KeyType.Jump: cInput.PressVirtualKey("Jump"); break;
                case KeyType.Escape: cInput.PressVirtualKey("Escape"); break;
            }
        } else {
            switch(keyType) {
                case KeyType.Left: cInput.ReleaseVirtualKey("Left"); break;
                case KeyType.Right: cInput.ReleaseVirtualKey("Right"); break;
                case KeyType.Jump: cInput.ReleaseVirtualKey("Jump"); break;
                case KeyType.Escape: cInput.ReleaseVirtualKey("Escape"); break;
            }
        }
    }
}
