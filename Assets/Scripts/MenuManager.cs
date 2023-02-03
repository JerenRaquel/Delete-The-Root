using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public GameObject linePrefab;
    public GameObject menuPrefabHolder;

    private Dictionary<string, ItemManager> items = new Dictionary<string, ItemManager>();

    public void Add(
        string itemName, Sprite icon, ItemManager.Callback callback,
        bool isInteractable = true) {
        if (this.items.ContainsKey(itemName)) return;

        GameObject go = Instantiate(linePrefab, menuPrefabHolder.transform);
        this.items.Add(itemName, go.GetComponent<ItemManager>());
        this.items[itemName].Initialize(itemName, icon, callback);
        this.items[itemName].Interactable = isInteractable;
    }

    public void Remove(string itemName) {
        if (!this.items.ContainsKey(itemName)) return;

        Destroy(this.items[itemName].gameObject);
        this.items.Remove(itemName);
    }

    public void SetCost(string itemName, int value) {
        this.items[itemName].SetCost(value);
    }

    public void Clear() {
        foreach (KeyValuePair<string, ItemManager> item in this.items) {
            Destroy(item.Value.gameObject);
        }
        this.items.Clear();
    }
}
