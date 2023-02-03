using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileDirectoryManager : MonoBehaviour {
    public Button deleteButton;
    public MenuManager fileExplorer;
    public GameObject parent;
    public TMPro.TextMeshProUGUI cdw;
    public GameObject[] keyParts;

    private Directory currentDirectory = null;
    private int keysFound = 0;

    public void Open() {
        this.parent.SetActive(true);
        this.currentDirectory = GameController.instance.PickRandomDirectory();
        foreach (var key in this.keyParts) {
            key.SetActive(false);
        }
        Display();
    }

    public void Close() {
        this.parent.SetActive(false);
        this.currentDirectory = null;
        this.keysFound = 0;
    }

    public void Delete() {
        if (!this.currentDirectory.IsRoot) return;
        GameController.instance.DeletedRoot();
    }

    public void Disconnect() {
        GameController.instance.DisconnectFromFileDir();
    }

    private void Display() {
        if (this.keysFound == 2) {
            deleteButton.interactable = true;
            return;
        }

        this.cdw.text = this.currentDirectory.Name;

        if (currentDirectory.HasParent) {
            fileExplorer.Add("..", () => {
                fileExplorer.Clear();
                currentDirectory = GameController.instance.GetDirectory(currentDirectory.GoUp());
                QuickHack();
                Display();
            });
        }

        if (!currentDirectory.HasVisited) {
            fileExplorer.Add("Key", () => {
                this.keyParts[this.keysFound].SetActive(true);
                this.keysFound++;
            });
        }

        if (this.currentDirectory == null) return;
        this.currentDirectory.Foreach((in string name, int index) => {
            if (GameController.instance.GetDirectory(name) == null || !GameController.instance.GetDirectory(name).HasChildren()) return;
            fileExplorer.Add(name, () => {
                string child = currentDirectory.GoDown(index);
                if (child == null) return;
                currentDirectory = GameController.instance.GetDirectory(child);
                fileExplorer.Clear();
                QuickHack();
                Display();
            });
        });
    }

    private void QuickHack() {
        if (this.currentDirectory.HasVisited) return;
        GameController.instance.LoadHackingMiniGame();
    }
}
