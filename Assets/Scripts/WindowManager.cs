using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour {
    private struct BufferData {
        public string itemName;
        public ItemManager.Callback callback;
    }

    public GameObject window;
    public MenuManager menuManager;
    public GameObject shopWindow;
    public DisplayUpgrades displayUpgrades;
    public Shop shop;
    public Sprite icon;
    public TMPro.TextMeshProUGUI cashBox;

    private Queue<BufferData> queue = new Queue<BufferData>();
    private Queue<string> removeBuffer = new Queue<string>();

    public void Close() {
        if (shopWindow != null) {
            this.shopWindow.SetActive(false);
        }
        this.window.SetActive(false);
    }

    public void Open(int slot = -1) {
        if (this.displayUpgrades != null) {
            if (PlayerProfiler.instance.GetUnequippedUpgrades() == null) return;
        }

        this.window.SetActive(true);
        if (this.cashBox != null) this.cashBox.text = "$" + PlayerProfiler.instance.cash;
        if (this.displayUpgrades != null && slot >= 0) {
            this.displayUpgrades.Display(slot);
        }
        if (this.menuManager == null) return;
        while (this.queue.Count > 0) {
            BufferData data = this.queue.Dequeue();
            this.menuManager.Add(data.itemName, icon, data.callback);
        }
        while (this.removeBuffer.Count > 0) {
            this.menuManager.Remove(this.removeBuffer.Dequeue());
        }
    }

    public void OpenShop() {
        if (shopWindow == null) return;
        this.shopWindow.SetActive(true);
        this.shop.UpdateCash();
    }

    public void Add(string itemName, ItemManager.Callback callback) {
        if (this.window.activeSelf) {
            this.menuManager.Add(itemName, icon, callback);
        } else {
            BufferData data;
            data.itemName = itemName;
            data.callback = callback;
            this.queue.Enqueue(data);
        }
    }

    public void Remove(string itemName) {
        if (this.window.activeSelf) {
            this.menuManager.Remove(itemName);
        } else {
            this.removeBuffer.Enqueue(itemName);
        }
    }
}
