using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowOpenListener : MonoBehaviour {
    [SerializeField] private WindowManager window;
    [SerializeField] private string messageTag;

    private void FixedUpdate() {
        if (this.window.IsOpen) {
            bool loaded = TutorialManager.instance.LoadMessage(this.messageTag);
            if (loaded) {
                Destroy(this);
            }
        }
    }
}
