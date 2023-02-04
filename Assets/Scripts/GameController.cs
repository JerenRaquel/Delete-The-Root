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
    public GameObject emailNotif;
    public EmailGenerator emailGenerator;
    public WindowManager emailManager;
    public GameObject emailViewerPrefab;
    public WindowManager finConnectManager;
    public FileDirectoryManager fileDirectory;

    private Dictionary<string, Email> recievedEmails;
    private string loadedHack = null;
    private int level = 1;

    private void Start() {
        this.recievedEmails = new Dictionary<string, Email>();
        SendEmail(this.emailGenerator.GetEmails(this.level));
    }

    public void RaiseAlert() {
        Debug.Log("ALERT!");
    }

    public void DisconnectFromFileDir() {
        RaiseAlert();
        this.fileDirectory.Close();
    }

    public void DeletedRoot(string ipv6) {
        PlayerProfiler.instance.cash += this.recievedEmails[ipv6].data.reward.cash;
        this.fileDirectory.Close();
        finConnectManager.Remove(ipv6);
        emailManager.Remove(this.recievedEmails[ipv6].data.subject);
        this.recievedEmails.Remove(ipv6);
        if (this.recievedEmails.Count == 0) {
            this.finConnectManager.Close();
            this.emailNotif.SetActive(false);
            this.level++;
            SendEmail(this.emailGenerator.GetEmails(this.level));
        }
    }

    public void HackComplete() {
        SceneHandler.instance.UnloadScene(loadedHack);
        this.cursor.SetActive(true);
        this.desktopScreen.SetActive(true);
    }

    public void LoadHackingMiniGame(string ipv6) {
        this.desktopScreen.SetActive(false);
        this.cursor.SetActive(false);
        this.loadedHack = this.recievedEmails[ipv6].HackScene;
        SceneHandler.instance.LoadScene(loadedHack);
    }

    private void SendEmail(Email[] emails) {
        if (emails == null) {
            WinGame();
            return;
        }
        if (emails.Length == 0) return;
        foreach (Email email in emails) {
            if (recievedEmails.ContainsKey(email.ipv6)) return;
            recievedEmails.Add(email.ipv6, email);
            this.emailNotif.SetActive(true);
            emailManager.Add(email.data.subject, () => {
                GameObject go = Instantiate(emailViewerPrefab, anchor.transform);
                go.GetComponent<EmailViewer>().Display(email);
            });
            CreateConnection(email);
        }
    }

    private void CreateConnection(Email email) {
        finConnectManager.Add(email.ipv6, () => {
            this.fileDirectory.Open(email);
        }, (ItemManager itemManager) => {
            itemManager.MarkAsSpecial();
        });
    }

    private void WinGame() {
        Debug.Log("You Win!");
    }
}
