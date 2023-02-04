using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonManager : MonoBehaviour {
    public Button button;

    private void OnEnable() {
        CheckButtonState();
    }

    public void CheckButtonState() {
        UpgradeData[] data = PlayerProfiler.instance.GetUnequippedUpgrades();
        if (data == null || data.Length == 0) {
            button.interactable = false;
        }
    }
}
