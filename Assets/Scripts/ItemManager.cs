using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {
    public delegate void Callback();

    public TMPro.TextMeshProUGUI textBox;
    public TMPro.TextMeshProUGUI costBox;
    public Color specialColor;
    public Image image;
    public Button button;

    private Callback callback;

    public bool Interactable {
        get { return this.button.interactable; }
        set { this.button.interactable = value; }
    }

    public ItemManager Initialize(string name, Sprite icon, Callback callback) {
        this.callback = callback;
        this.textBox.text = name;
        if (icon != null) {
            this.image.sprite = icon;
        }
        return this;
    }

    public ItemManager MarkAsSpecial() {
        this.textBox.color = this.specialColor;
        return this;
    }

    public void SetCost(int cost) {
        if (this.costBox == null) return;
        this.costBox.text = "$" + cost.ToString();
    }

    public string GetName() {
        return this.textBox.text;
    }

    public void Invoke() {
        if (this.callback != null) {
            this.callback();
        }
    }
}
