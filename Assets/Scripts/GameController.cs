using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EmailGenerator))]
public class GameController : MonoBehaviour {
    public Canvas canvas;
    public EmailGenerator emailGenerator;
    public WindowManager emailManager;
    public GameObject emailViewerPrefab;

    private List<Email> emails;

    private void Start() {
        emails = new List<Email>();
        for (int i = 0; i < 4; i++) {
            Email email = this.emailGenerator.GetEmail(
                EmailGenerator.EmailGeneratorType.HACKGRID,
                EmailDifficulty.Difficulty.EASY
            );
            emails.Add(email);
            emailManager.Add(email.data.subject, () => {
                GameObject go = Instantiate(emailViewerPrefab, canvas.transform);
                go.GetComponent<EmailViewer>().Display(email);
                Debug.Log("Displaying");
            });
        }
    }
}
