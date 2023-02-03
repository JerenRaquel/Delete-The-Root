using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EmailGenerator))]
public class GameController : MonoBehaviour {
    #region Class Instance
    public static GameController instance = null;
    private void CreateInstance()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        }
    #endregion
    private void Awake() => CreateInstance();

    public GameObject anchor;
    public GameObject cursor;
    public GameObject desktopScreen;
    public EmailGenerator emailGenerator;
    public WindowManager emailManager;
    public GameObject emailViewerPrefab;
    public WindowManager finConnectManager;
    public FileDirectoryManager fileDirectory;
    public string[] possibleRoots;
    public DirectoryData[] directoryTemplates;

    private Dictionary<string, Email> emails;
    private GameObject activeFileDirectory;
    private Dictionary<string, Directory> directories;
    private string rootDirectory = null;

    private void Start() {
        this.emails = new Dictionary<string, Email>();
        this.directories = new Dictionary<string, Directory>();
        SetupDirectories();
        for (int i = 0; i < 4; i++) {
            Email email = this.emailGenerator.GetEmail(
                EmailGenerator.EmailGeneratorType.HACKGRID,
                EmailDifficulty.Difficulty.EASY
            );
            SendEmail(email);
        }
    }

    public void DisconnectFromFileDir() {
        // Set Alert Flags
        this.directories[rootDirectory].Reset();
        this.fileDirectory.Close();
    }

    public void DeletedRoot() {
        // Claim rewards
        this.directories[rootDirectory].Reset();
        this.fileDirectory.Close();
    }

    public Directory GetDirectory(string name) {
        if (!this.directories.ContainsKey(name)) return null;
        return this.directories[name];
    }

    public Directory PickRandomDirectory() {
        int keyIndex = Random.Range(0, this.possibleRoots.Length);
        this.rootDirectory = this.possibleRoots[keyIndex];
        this.directories[this.possibleRoots[keyIndex]].SetAsRoot();
        return this.directories[this.possibleRoots[keyIndex]];
    }

    public void HackComplete() {
        SceneHandler.instance.UnloadScene("GridHack");
        this.cursor.SetActive(true);
        this.desktopScreen.SetActive(true);
    }

    public void LoadHackingMiniGame() {
        this.desktopScreen.SetActive(false);
        this.cursor.SetActive(false);
        SceneHandler.instance.LoadScene("GridHack");
    }

    private void SetupDirectories() {
        foreach (DirectoryData directory in this.directoryTemplates) {
            bool state = this.directories.TryAdd(directory.name, new Directory(directory));
        }
    }

    private void SendEmail(Email email) {
        if (emails.ContainsKey(email.ipv6)) return;
        emails.Add(email.ipv6, email);
        emailManager.Add(email.data.subject, () => {
            GameObject go = Instantiate(emailViewerPrefab, anchor.transform);
            go.GetComponent<EmailViewer>().Display(email);
            Debug.Log("Displaying");
        });
        CreateConnection(email.ipv6);
    }

    private void CreateConnection(string ipv6) {
        finConnectManager.Add(ipv6, () => {
            this.fileDirectory.Open();
        });
    }
}
