using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {
    #region Class Instance
    public static TutorialManager instance = null;
    private void CreateInstance() {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion

    [SerializeField] private GameObject defaultBlocker;
    [SerializeField] private GameObject chatMessagePrefab;
    [SerializeField] private Transform chatAnchor;
    [SerializeField] private GameObject mouseBlocker;
    [Header("Message Data")]
    [SerializeField] private string[] messageTriggers;
    [SerializeField] private Message[] messages;

    private Dictionary<string, Message> messageData;
    private Dictionary<string, bool> triggers;
    private MessageManager currentOpenMessage = null;
    private InputActions inputActions;

    [HideInInspector]
    public bool IsTutorialActive {
        get { return this.currentOpenMessage != null; }
    }

    private void Awake() {
        CreateInstance();
        this.inputActions = new InputActions();
        this.messageData = new Dictionary<string, Message>();
        this.triggers = new Dictionary<string, bool>();

        foreach (Message message in this.messages) {
            this.messageData.TryAdd(message.messageTag, message);
        }
        foreach (string messageTag in this.messageTriggers) {
            this.triggers.TryAdd(messageTag, false);
        }
    }

    private void OnEnable() {
        this.inputActions.Enable();
        this.inputActions.Messager.NextMessage.performed += _ => this.PlayNextMessage();
    }

    private void OnDisable() {
        this.inputActions.Messager.NextMessage.performed -= _ => this.PlayNextMessage();
        this.inputActions.Disable();
    }

    public bool LoadMessage(string messageTag) {
        if (this.IsTutorialActive) return false;
        if (!this.messageData.ContainsKey(messageTag)) return false;
        if (this.triggers[messageTag]) return false;

        this.triggers[messageTag] = true;
        Message message = this.messageData[messageTag];
        if (message.blocker == null) {
            this.defaultBlocker.SetActive(true);
        } else {
            message.blocker.SetActive(true);
        }
        this.mouseBlocker.SetActive(true);

        GameObject go = Instantiate(chatMessagePrefab, chatAnchor);
        this.currentOpenMessage = go.GetComponent<MessageManager>().Initialize(
            this.messageData[messageTag].parts,
            this.messageData[messageTag].sender,
            () => {
                Destroy(this.currentOpenMessage.gameObject);
                if (message.blocker == null) {
                    this.defaultBlocker.SetActive(false);
                } else {
                    message.blocker.SetActive(false);
                }
                this.mouseBlocker.SetActive(false);
            }
        ).Play();
        return true;
    }

    private void PlayNextMessage() {
        this.currentOpenMessage.Play();
    }
}
