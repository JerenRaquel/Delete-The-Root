using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileHandler : MonoBehaviour {
    [SerializeField] private TMPro.TextMeshProUGUI mainBox;
    [SerializeField] private TMPro.TextMeshProUGUI sideBox;

    private void OnEnable() {
        this.mainBox.text = "Main Missions Completed:\n" + PlayerProfiler.instance.mainMissionsDone.ToString();
        this.sideBox.text = "Side Missions Completed:\n" + PlayerProfiler.instance.sideMissionsDone.ToString();
    }
}
