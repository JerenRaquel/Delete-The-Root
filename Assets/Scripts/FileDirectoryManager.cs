using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileDirectoryManager : MonoBehaviour {
    public MenuManager fileExplorer;
    public GameObject parent;
    public TMPro.TextMeshProUGUI cdw;

    private Directory currentDirectory = null;

    public void Open() {
        this.parent.SetActive(true);
        this.currentDirectory = GameController.instance.PickRandomDirectory();
        Display();
    }

    public void Close() {
        this.parent.SetActive(false);
        this.currentDirectory = null;
    }

    public void Delete() {
        if (!this.currentDirectory.IsRoot) return;
        GameController.instance.DeletedRoot();
    }

    public void Disconnect() {
        GameController.instance.DisconnectFromFileDir();
    }

    private void Display() {
        this.cdw.text = this.currentDirectory.Name;

        if (currentDirectory.HasParent) {
            fileExplorer.Add("..", () => {
                fileExplorer.Clear();
                currentDirectory = GameController.instance.GetDirectory(currentDirectory.GoUp());
                QuickHack();
                Display();
            });
        }

        if (this.currentDirectory == null) return;
        this.currentDirectory.Foreach((in string name, int index) => {
            if(GameController.instance.GetDirectory(name) == null || !GameController.instance.GetDirectory(name).HasChildren()) return;
            fileExplorer.Add(name, () => {
                string child = currentDirectory.GoDown(index);
                if (child == null) {
                    Debug.Log("HI");
                    return;
                }
                currentDirectory = GameController.instance.GetDirectory(child);
                fileExplorer.Clear();
                QuickHack();
                Display();
            });
        });
    }

    private void QuickHack() {
        if (this.currentDirectory.HasVisited) return;
        // Debug.Log("Begin Hacking");
        GameController.instance.LoadHackingMiniGame();
    }
}
