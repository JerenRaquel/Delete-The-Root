using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUpgrades : MonoBehaviour {
    public MenuManager upgradeMenu;
    public Button[] slots;

    public void Display(int slot) {
        int count = 0;
        for (int i = 0; i < 3; i++) {
            Sprite icon = PlayerProfiler.instance.GetEquippedSprite(i);
            if (icon != null) {
                this.slots[i].gameObject.GetComponent<Image>().sprite = icon;
                count++;
            }
        }
        if (count == 3) return;

        foreach (var item in PlayerProfiler.instance.GetUnequippedUpgrades()) {
            this.upgradeMenu.Add(item.name, item.icon, () => {
                PlayerProfiler.instance.ReplaceUpgrade(item.name, slot);
                this.upgradeMenu.Remove(item.name);
                this.slots[slot].gameObject.GetComponent<Image>().sprite = item.icon;
                if (PlayerProfiler.instance.GetUnequippedUpgrades() == null) {
                    foreach (var button in this.slots) {
                        button.gameObject.GetComponent<UpgradeButtonManager>().CheckButtonState();
                    }
                }
            });
        }
    }

    public void Clear() {
        this.upgradeMenu.Clear();
    }
}
