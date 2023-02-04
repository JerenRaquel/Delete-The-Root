using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {
    [SerializeField] private GameObject defaultBlocker;
    [SerializeField] private GameObject chatMessagePrefab;
    [SerializeField] private Transform chatAnchor;
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

    public void LoadMessage(string messageTag) {
        if (this.IsTutorialActive) return;
        if (!this.messageData.ContainsKey(messageTag)) return;
        if (this.triggers[messageTag]) return;

        this.triggers[messageTag] = true;
        Message message = this.messageData[messageTag];
        if (message.blocker == null) {
            this.defaultBlocker.SetActive(true);
        } else {
            message.blocker.SetActive(true);
        }

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
            }
        ).Play();
    }

    private void PlayNextMessage() {
        this.currentOpenMessage.Play();
    }
}
