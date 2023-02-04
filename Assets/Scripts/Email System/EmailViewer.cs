using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailViewer : MonoBehaviour {
    public TMPro.TextMeshProUGUI subjectBox;
    public TMPro.TextMeshProUGUI messageBox;
    public TMPro.TextMeshProUGUI senderBox;

    public void Display(Email email) {
        this.subjectBox.text = email.data.subject;
        this.messageBox.text = email.data.message;
        this.senderBox.text = email.data.sender;
    }

    public void Close() {
        Destroy(this.gameObject);
    }
}
