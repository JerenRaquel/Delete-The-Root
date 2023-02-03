using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileDirectoryManager : MonoBehaviour {
    public MenuManager fileExplorer;

    private GameController gameController;
    private Directory currentDirectory = null;

    public void Initialize(GameController controller, DirectoryData directoryData) {
        this.gameController = controller;
        this.currentDirectory = new Directory(directoryData);
    }

    public void Delete() {
        if (!this.currentDirectory.IsRoot) return;
        this.gameController.DeletedRoot();
    }

    public void Disconnect() {
        this.gameController.DisconnectFromFileDir();
    }

    private void Display() {
        fileExplorer.Add("..", () => {
            fileExplorer.Clear();
            currentDirectory = currentDirectory.GoUp();
            QuickHack();
            Display();
        });

        if (this.currentDirectory == null) return;
        this.currentDirectory.Foreach((in string name, int index) => {
            fileExplorer.Add(name, () => {
                fileExplorer.Clear();
                currentDirectory = currentDirectory.GoDown(index);
                QuickHack();
                Display();
            });
        });
    }

    private void QuickHack() {
        if (this.currentDirectory.HasVisited) return;
        Debug.Log("Begin Hacking");
    }
}
