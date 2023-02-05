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
    public Slider alertBar;
    public GameObject alertBarParent;
    public GameObject alertBarPlaceholder;
    public Image[] keyParts;
    public Sprite[] foundParts;
    public Sprite[] outlineParts;

    private FileSystem fileSystem;
    [HideInInspector] public Email email { get; private set; }
    private bool requiresUpdate = false;

    public void Open(Email email) {
        this.parent.SetActive(true);
        this.email = email;
        this.fileSystem = new FileSystem(
            email.data.rootFolder.directoryData,
            email.data.rootFolder.root
        );
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
        Reset();
        GameController.instance.DeletedRoot(this.email.ipv6);
    }

    public void Disconnect() {
        Reset();
        GameController.instance.DisconnectFromFileDir(this.email.ipv6);
    }

    public void Reset() {
        this.fileSystem = null;
        this.fileExplorer.Clear();
        this.requiresUpdate = false;
    }

    private void FixedUpdate() {
        if (this.requiresUpdate && this.fileSystem != null) {
            this.requiresUpdate = false;
            Display();
        }
    }

    private void Display() {
        deleteButton.interactable = this.fileSystem.IsUnlocked;
        this.cdw.text = this.fileSystem.DirectoryName;
        LoadKey();
        LoadParent();
        LoadDirectories();
        int alertLevel = AlertManager.instance.GetAlertLevel(this.email.ipv6);
        if (alertLevel > 0) {
            this.alertBarParent.SetActive(true);
            this.alertBarPlaceholder.SetActive(false);
            this.alertBar.value = alertLevel;
        } else {
            this.alertBarParent.SetActive(false);
            this.alertBarPlaceholder.SetActive(true);
        }
    }

    private void QuickHack() {
        if (this.fileSystem.HasVisited) return;
        if (!this.fileSystem.HasKey) {
            int level = AlertManager.instance.GetAlertLevel(this.email.ipv6);
            int rng;
            if (level > 1) {
                rng = Random.Range(0, 4);
            } else {
                rng = Random.Range(0, 2);
            }
            if (rng == 1) return;
        }
        GameController.instance.LoadHackingMiniGame(this.email.ipv6);
    }

    private void LoadKey() {
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

    private void LoadParent() {
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
    }

    private void LoadDirectories() {
        this.fileSystem.Foreach((in string name, int index) => {
            string child = this.fileSystem.GetChild(index);
            if (child == null || this.fileSystem.CheckIfFile(child)) {
                this.fileExplorer.Add(name, fileIcon, null, false);
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
    }
}
