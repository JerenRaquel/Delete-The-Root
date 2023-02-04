using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicProgressWrapper : MonoBehaviour {
    [SerializeField] private Slider progressBar;

    private void OnEnable() {
        this.progressBar.direction = Slider.Direction.LeftToRight;
        this.progressBar.minValue = 0;
        this.progressBar.maxValue = MusicPlayer.instance.GetSongLength();
    }

    private void Update() {
        this.progressBar.value = MusicPlayer.instance.GetSongTime();
    }
}
