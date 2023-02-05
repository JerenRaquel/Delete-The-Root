using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    [SerializeField] private MenuManager shopMenu;
    [SerializeField] private TMPro.TextMeshProUGUI cashBox;
    [SerializeField] private Button[] upgradeButtons;

    private HashSet<string> buyableUpgrades;

    private void Start() {
        this.buyableUpgrades = new HashSet<string>();
        foreach (var item in PlayerProfiler.instance.GetAllUpgradeData()) {
            this.buyableUpgrades.Add(item.name);
            this.shopMenu.Add(item.name, item.icon, () => {
                if (PlayerProfiler.instance.cash < item.cost) return;
                PlayerProfiler.instance.cash -= item.cost;
                UpdateCash();
                this.shopMenu.Remove(item.name);
                int slotID = PlayerProfiler.instance.AddUpgrade(item.name);
                if (slotID >= 0) {
                    this.upgradeButtons[slotID]
                        .gameObject
                        .GetComponent<Image>()
                        .sprite = item.icon;
                }
                UpgradeData[] unequippedUpgrades = PlayerProfiler.instance.GetUnequippedUpgrades();
                if (unequippedUpgrades == null || unequippedUpgrades.Length <= 0) return;
                foreach (var button in this.upgradeButtons) {
                    button.interactable = true;
                }
            });
            this.shopMenu.SetCost(item.name, item.cost);
        }
    }

    public void UpdateCash() {
        this.cashBox.text = "$" + PlayerProfiler.instance.cash.ToString();
    }
}
