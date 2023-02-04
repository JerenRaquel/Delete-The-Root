using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeWrapper : MonoBehaviour {
    [SerializeField] private TMPro.TextMeshProUGUI volumeBox;
    [SerializeField] private Slider volumeSlider;

    public void UpdateMusicPlayer() {
        float value = this.volumeSlider.value;
        this.volumeBox.text = (Mathf.RoundToInt(value * 100)).ToString() + "%";
        MusicPlayer.instance.ChangeVolumeLevel(value);
    }
}
