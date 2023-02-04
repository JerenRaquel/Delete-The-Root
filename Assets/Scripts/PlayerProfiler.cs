using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfiler : MonoBehaviour {
    public delegate void Enumerate(string name);

    #region Class Instance
    public static PlayerProfiler instance = null;
    private void CreateInstance() {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion
    private void Awake() => CreateInstance();

    [SerializeField] private UpgradeData[] upgrades;
    public int cash;

    private Dictionary<string, UpgradeData> upgradeDict;
    private HashSet<string> boughtUpgrades;
    private HashSet<string> equipedUpgrades;
    private string[] slots;
    [HideInInspector] public int mainMissionsDone = 0;
    [HideInInspector] public int sideMissionsDone = 0;

    private void Start() {
        this.upgradeDict = new Dictionary<string, UpgradeData>();
        this.boughtUpgrades = new HashSet<string>();
        this.equipedUpgrades = new HashSet<string>();
        this.slots = new string[3];

        foreach (var item in this.upgrades) {
            this.upgradeDict.Add(item.name, item);
        }
    }

    public void AddUpgrade(string name) {
        if (this.boughtUpgrades.Contains(name)) return;
        this.boughtUpgrades.Add(name);
        for (int i = 0; i < 3; i++) {
            if (this.slots[i] == null || this.slots[i] == "") {
                this.slots[i] = name;
                this.equipedUpgrades.Add(name);
            }
        }
    }

    public void EquipUprade(string name, int slot) {
        if (!this.boughtUpgrades.Contains(name)) return;
        if (this.equipedUpgrades.Contains(name)) return;
        if (this.equipedUpgrades.Count >= 3) return;

        this.equipedUpgrades.Add(name);
        this.slots[slot] = name;
    }

    public void UnequipUpgrade(int slot) {
        string name = this.GetEquippedUpgrade(slot);
        if (!this.equipedUpgrades.Contains(name)) return;

        this.equipedUpgrades.Remove(name);
        this.slots[slot] = null;
    }

    public void ReplaceUpgrade(string newName, int slotID) {
        UnequipUpgrade(slotID);
        EquipUprade(newName, slotID);
    }

    public void AccessUpgrades(Enumerate func) {
        foreach (string upgrade in this.equipedUpgrades) {
            func(upgrade);
        }
    }

    public bool IsEquiped(string name) {
        return this.equipedUpgrades.Contains(name);
    }

    public UpgradeData[] GetUnequippedUpgrades() {
        int remaining = this.boughtUpgrades.Count - this.equipedUpgrades.Count;
        if (remaining <= 0) return null;
        UpgradeData[] result = new UpgradeData[remaining];
        int i = 0;
        foreach (var item in this.boughtUpgrades) {
            if (!this.equipedUpgrades.Contains(item)) {
                result[i] = this.upgradeDict[item];
                i++;
            }
        }
        return result;
    }

    public UpgradeData[] GetAllUpgradeData() {
        return this.upgrades;
    }

    public Sprite GetEquippedSprite(int slot) {
        if (slot < 0 || slot > 2) return null;
        if (this.slots[slot] == null || this.slots[slot] == "") return null;
        return this.upgradeDict[this.slots[slot]].icon;
    }

    public string GetEquippedUpgrade(int slot) {
        if (slot < 0 || slot > 2) return null;
        return this.slots[slot];
    }

    private int GetSlot(string name) {
        if (!this.equipedUpgrades.Contains(name)) return -1;
        for (int i = 0; i < 3; i++) {
            if (this.slots[i] == name) return i;
        }
        return -1;
    }
}
