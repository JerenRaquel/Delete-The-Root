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

    private Queue<BufferData> queue = new Queue<BufferData>();

    public void Close() {
        this.window.SetActive(false);
    }

    public void Open() {
        this.window.SetActive(true);
        while (this.queue.Count > 0) {
            BufferData data = this.queue.Dequeue();
            this.menuManager.Add(data.itemName, data.callback);
        }
    }

    public void Add(string itemName, ItemManager.Callback callback) {
        if (this.window.activeSelf) {
            this.menuManager.Add(itemName, callback);
        } else {
            BufferData data;
            data.itemName = itemName;
            data.callback = callback;
            this.queue.Enqueue(data);
        }
    }
}
