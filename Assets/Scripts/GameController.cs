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

    private Dictionary<string, Email> emails;

    private void Start() {
        this.emails = new Dictionary<string, Email>();
        SendEmail(this.emailGenerator.GetEmail(
            EmailGenerator.EmailGeneratorType.HACKGRID,
            EmailDifficulty.Difficulty.EASY
        ));
    }

    public void DisconnectFromFileDir() {
        // Set Alert Flags
        this.fileDirectory.Close();
    }

    public void DeletedRoot(string ipv6) {
        // Claim rewards
        Debug.Log("WON");
        this.fileDirectory.Close();
        finConnectManager.Remove(ipv6);
        emailManager.Remove(this.emails[ipv6].data.subject);
        this.emails.Remove(ipv6);
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

    private void SendEmail(Email email) {
        if (emails.ContainsKey(email.ipv6)) return;
        emails.Add(email.ipv6, email);
        emailManager.Add(email.data.subject, () => {
            GameObject go = Instantiate(emailViewerPrefab, anchor.transform);
            go.GetComponent<EmailViewer>().Display(email);
        });
        CreateConnection(email);
    }

    private void CreateConnection(Email email) {
        finConnectManager.Add(email.ipv6, () => {
            this.fileDirectory.Open(email);
        });
    }
}
