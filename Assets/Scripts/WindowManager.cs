using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour {
    public delegate void Apply(ItemManager itemManager);

    private struct BufferData {
        public string itemName;
        public ItemManager.Callback callback;
        public Apply itemApplication;
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

    [HideInInspector] public bool IsOpen { get; private set; }

    public void Close() {
        this.IsOpen = false;
        if (shopWindow != null) {
            this.shopWindow.SetActive(false);
        }
        this.window.SetActive(false);
    }

    public void Open(int slot = -1) {
        this.IsOpen = true;
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
            ItemManager im = this.menuManager.Add(data.itemName, icon, data.callback);
            if (data.itemApplication != null) data.itemApplication(im);
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

    public void Add(string itemName, ItemManager.Callback callback, Apply applyFunc = null) {
        if (this.window.activeSelf) {
            this.menuManager.Add(itemName, icon, callback);
        } else {
            BufferData data;
            data.itemName = itemName;
            data.callback = callback;
            data.itemApplication = applyFunc;
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
