using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public GameObject linePrefab;
    public GameObject menuPrefabHolder;
    public Sprite icon;

    private Dictionary<string, ItemManager> items = new Dictionary<string, ItemManager>();

    public void Add(string itemName, ItemManager.Callback callback) {
        if (this.items.ContainsKey(itemName)) return;

        GameObject go = Instantiate(linePrefab, menuPrefabHolder.transform);
        this.items.Add(itemName, go.GetComponent<ItemManager>());
        this.items[itemName].Initialize(itemName, this.icon, callback);
    }

    public void Remove(string itemName) {
        if (!this.items.ContainsKey(itemName)) return;

        Destroy(this.items[itemName].gameObject);
        this.items.Remove(itemName);
    }
}
