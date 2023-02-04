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
    private void Awake() {
        CreateInstance();
        this.inputActions = new InputActions();
    }

    public GameObject anchor;
    public GameObject cursor;
    public GameObject desktopScreen;
    public GameObject emailNotif;
    public EmailGenerator emailGenerator;
    public WindowManager emailManager;
    public GameObject emailViewerPrefab;
    public WindowManager finConnectManager;
    public FileDirectoryManager fileDirectory;
    public Transform chatAnchor;
    public GameObject chatBlocker;
    public GameObject chatMessagePrefab;
    public string startingMessage;
    public string firstHackMessage;
    public string firstAlertLevelMessage;
    public string firstConnectionMessage;
    public Message[] messages;

    private Dictionary<string, Email> recievedEmails;
    private Dictionary<string, Message> messageData;
    private MessageManager currentOpenMessage = null;
    private InputActions inputActions;
    private string loadedHack = null;
    private int level = 0;
    private bool firstHack = true;
    private bool firstAlertLevel = true;
    private bool firstConnect = true;

    [HideInInspector] public int AlertLevel { get; private set; } = 0;
    [HideInInspector] public bool ActiveTutorialWorking { get { return this.currentOpenMessage != null; } }

    private void OnEnable() {
        this.inputActions.Enable();
        this.inputActions.Messager.NextMessage.performed += _ => this.PlayNextMessage();
    }

    private void OnDisable() {
        this.inputActions.Messager.NextMessage.performed -= _ => this.PlayNextMessage();
        this.inputActions.Disable();
    }

    private void Start() {
        this.recievedEmails = new Dictionary<string, Email>();
        this.messageData = new Dictionary<string, Message>();

        foreach (Message message in this.messages) {
            this.messageData.TryAdd(message.messageTag, message);
        }

        SendEmail(this.emailGenerator.GetEmails(this.level));

        this.currentOpenMessage = LoadMessage(startingMessage);
    }

    public void RaiseAlert() {
        this.AlertLevel++;
    }

    public void DisconnectFromFileDir() {
        RaiseAlert();
        this.fileDirectory.Close();
    }

    public void DeletedRoot(string ipv6) {
        PlayerProfiler.instance.cash += this.recievedEmails[ipv6].data.reward.cash;
        if (this.recievedEmails[ipv6].data.mainMission) {
            PlayerProfiler.instance.mainMissionsDone++;
        } else {
            PlayerProfiler.instance.sideMissionsDone++;
        }
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
        if (this.firstAlertLevel) {
            this.firstAlertLevel = false;
            this.currentOpenMessage = LoadMessage(this.firstAlertLevelMessage);
        }
    }

    public void LoadHackingMiniGame(string ipv6) {
        this.desktopScreen.SetActive(false);
        this.cursor.SetActive(false);
        this.loadedHack = this.recievedEmails[ipv6].HackScene;
        SceneHandler.instance.LoadScene(loadedHack);
        if (this.firstHack) {
            this.firstHack = false;
            this.currentOpenMessage = LoadMessage(firstHackMessage);
        }
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
            if (this.firstConnect) {
                this.firstConnect = false;
                this.currentOpenMessage = LoadMessage(this.firstConnectionMessage);
            }
        }, (ItemManager itemManager) => {
            if (email.data.mainMission) itemManager.MarkAsSpecial();
        });
    }

    private void WinGame() {
        Debug.Log("You Win!");
    }

    private MessageManager LoadMessage(string messageTag) {
        this.chatBlocker.SetActive(true);
        GameObject go = Instantiate(chatMessagePrefab, chatAnchor);
        return go.GetComponent<MessageManager>().Initialize(
            this.messageData[messageTag].parts,
            this.messageData[messageTag].sender,
            () => {
                Destroy(this.currentOpenMessage.gameObject);
                this.chatBlocker.SetActive(false);
            }
        ).Play();
    }

    private void PlayNextMessage() {
        if (this.currentOpenMessage == null) return;
        this.currentOpenMessage.Play();
    }
}
