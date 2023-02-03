using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EmailGenerator))]
public class GameController : MonoBehaviour {
    public Canvas canvas;
    public EmailGenerator emailGenerator;
    public WindowManager emailManager;
    public GameObject emailViewerPrefab;
    public WindowManager finConnectManager;
    public GameObject fileDirectory;

    private Dictionary<string, Email> emails;
    private GameObject activeFileDirectory;

    private void Start() {
        emails = new Dictionary<string, Email>();
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
        Destroy(activeFileDirectory);
    }

    public void DeletedRoot() {
        // Claim rewards
        Destroy(activeFileDirectory);
    }

    private void SendEmail(Email email) {
        if(emails.ContainsKey(email.ipv6)) return;
        emails.Add(email.ipv6, email);
        emailManager.Add(email.data.subject, () => {
            GameObject go = Instantiate(emailViewerPrefab, canvas.transform);
            go.GetComponent<EmailViewer>().Display(email);
            Debug.Log("Displaying");
        });
        CreateConnection(email.ipv6);
    }

    private void CreateConnection(string ipv6) {
        finConnectManager.Add(ipv6, () => {
            this.activeFileDirectory = Instantiate(
                this.fileDirectory, this.canvas.transform
            );
        });
    }
}
