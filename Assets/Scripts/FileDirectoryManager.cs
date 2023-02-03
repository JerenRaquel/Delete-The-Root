using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileDirectoryManager : MonoBehaviour {
    public Button deleteButton;
    public MenuManager fileExplorer;
    public GameObject parent;
    public TMPro.TextMeshProUGUI cdw;
    public Sprite folderIcon;
    public Sprite fileIcon;
    public Sprite keyIcon;
    public Image[] keyParts;
    public Sprite[] foundParts;
    public Sprite[] outlineParts;

    private FileSystem fileSystem;
    private Email email;
    private bool requiresUpdate = false;

    public void Open(Email email) {
        this.parent.SetActive(true);
        this.email = email;
        this.fileSystem = new FileSystem(email.data.directoryData, email.data.root);
        for (int i = 0; i < this.keyParts.Length; i++) {
            this.keyParts[i].sprite = this.outlineParts[i];
        }
        Display();
    }

    public void Close() {
        this.parent.SetActive(false);
    }

    public void Delete() {
        if (!this.fileSystem.IsUnlocked) return;
        GameController.instance.DeletedRoot(this.email.ipv6);
    }

    public void Disconnect() {
        this.fileSystem = null;
        this.email = null;
        this.fileExplorer.Clear();
        GameController.instance.DisconnectFromFileDir();
    }

    private void FixedUpdate() {
        if (this.requiresUpdate) {
            this.requiresUpdate = false;
            Display();
        }
    }

    private void Display() {
        deleteButton.interactable = this.fileSystem.IsUnlocked;
        this.cdw.text = this.fileSystem.DirectoryName;

        string parent = this.fileSystem.GetParent();
        if (parent != null) {
            fileExplorer.Add("..", folderIcon, () => {
                this.fileSystem.ChangeDirectory(parent, () => {
                    this.fileExplorer.Clear();
                    QuickHack();
                    this.requiresUpdate = true;
                });
            });
        }

        this.fileSystem.Foreach((in string name, int index) => {
            string child = this.fileSystem.GetChild(index);
            if (child == null || this.fileSystem.CheckIfFile(child)) {
                this.fileExplorer.Add(name, fileIcon, null);
            } else {
                this.fileExplorer.Add(name, folderIcon, () => {
                    this.fileSystem.ChangeDirectory(child, () => {
                        this.fileExplorer.Clear();
                        QuickHack();
                        this.requiresUpdate = true;
                    });
                });
            }
        });

        if (this.fileSystem.HasKey) {
            this.fileExplorer.Add("Key", this.keyIcon, () => {
                this.keyParts[this.fileSystem.Keys].sprite = this.foundParts[this.fileSystem.Keys];
                this.fileSystem.FoundKey();
                this.fileExplorer.Remove("Key");
                this.fileSystem.MarkAsVisited();
                this.requiresUpdate = true;
            });
        }
    }

    private void QuickHack() {
        if (this.fileSystem.HasVisited) return;
        GameController.instance.LoadHackingMiniGame();
    }
}
