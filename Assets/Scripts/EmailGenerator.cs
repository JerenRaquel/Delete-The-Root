using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Email {
    public EmailData data;
    public bool markedAsRead;
    public string ipv6;

    public Email(EmailData emailData) {
        this.data = emailData;
    }

    public string HackScene {
        get {
            switch (this.data.hackingType) {
                case EmailGenerator.EmailGeneratorType.ANY:
                    return "GridHack";
                case EmailGenerator.EmailGeneratorType.HACKGRID:
                    return "GridHack";
                case EmailGenerator.EmailGeneratorType.MATRIXCYPER:
                    return null;
            default:
                return null;
            }
        }
    }
}

public class EmailGenerator : MonoBehaviour {
    [System.Serializable]
    public enum EmailGeneratorType {
        ANY = HACKGRID | MATRIXCYPER,
        HACKGRID = 0b0000_0001,
        MATRIXCYPER = 0b0000_0010
    }

    [SerializeField] private EmailData[] emails;

    private Dictionary<int, List<Email>> missionEmails;
    private int maxLevel;

    private void Awake() {
        this.missionEmails = new Dictionary<int, List<Email>>();
        foreach (EmailData email in this.emails) {
            if (!this.missionEmails.ContainsKey(email.level)) {
                this.missionEmails.Add(email.level, new List<Email>());
                this.maxLevel = Mathf.Max(this.maxLevel, email.level);
            }
            this.missionEmails[email.level].Add(new Email(email));
        }
    }

    public Email[] GetEmails(int level) {
        if (level >= this.maxLevel) return null;
        Email[] fetchedMail = this.missionEmails[level].ToArray();
        foreach (Email email in fetchedMail) {
            email.ipv6 = GenerateHash(email.data.subject);
        }
        return fetchedMail;
    }

    private string GenerateHash(string subject) {
        Hash128 hash = new Hash128();
        hash.Append(subject);
        string hashStr = hash.ToString();
        string ipv6 = "";
        for(int i = 0; i < hashStr.Length; i++) {
            ipv6 += hashStr[i];
            if(i != 0 && i % 4 == 0){
                ipv6 += ":";
            }
        }
        return ipv6;
    }
}
