using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {
    public delegate void Callback();

    public TMPro.TextMeshProUGUI textBox;
    public Image image;
    public Button button;

    private Callback callback;

    public bool Interactable {
        get { return this.button.interactable; }
        set { this.button.interactable = value; }
    }

    public void Initialize(string name, Sprite icon, Callback callback) {
        this.callback = callback;
        this.textBox.text = name;
        if (icon != null) {
            this.image.sprite = icon;
        }
    }

    public void Invoke() {
        if (this.callback != null) {
            this.callback();
        }
    }
}
