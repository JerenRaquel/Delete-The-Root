using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTitleWrapper : MonoBehaviour {
    public TMPro.TextMeshProUGUI musicTitleBox;

    public void UpdateTitle(string song) {
        this.musicTitleBox.text = song;
    }
}
