using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Message {
    public string sender;
    public string messageTag;
    public GameObject blocker;
    public TextSystem.TextData[] parts;
}

public class MessageManager : MonoBehaviour {
    public delegate void Callback();

    public TMPro.TextMeshProUGUI senderBox;
    public TMPro.TextMeshProUGUI instructionBox;
    public TextSystem.TextScroller scroller;

    private Callback callback;

    private void FixedUpdate() {
        if (this.scroller.IsFinished) {
            this.instructionBox.text = "Tab To close...";
        } else if (!this.scroller.IsWorking) {
            this.instructionBox.text = "Tab To keep reading...";
        }
    }

    public MessageManager Initialize(TextSystem.TextData[] text, string sender, Callback callback) {
        this.senderBox.text = sender;
        this.scroller.textChunks = text;
        this.callback = callback;
        return this;
    }

    public MessageManager Play() {
        if (this.scroller.IsFinished && this.callback != null) {
            this.callback();
            return this;
        }
        this.scroller.Read();
        this.instructionBox.text = "";
        return this;
    }
}
