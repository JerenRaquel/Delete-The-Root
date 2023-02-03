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
                if (PlayerProfiler.instance.cash >= item.cost) {
                    PlayerProfiler.instance.cash -= item.cost;
                    this.shopMenu.Remove(item.name);
                    PlayerProfiler.instance.AddUpgrade(item.name);
                    foreach (var button in this.upgradeButtons) {
                        button.interactable = true;
                    }
                    UpdateCash();
                }
            });
            this.shopMenu.SetCost(item.name, item.cost);
        }
    }

    public void UpdateCash() {
        this.cashBox.text = "$" + PlayerProfiler.instance.cash.ToString();
    }
}
