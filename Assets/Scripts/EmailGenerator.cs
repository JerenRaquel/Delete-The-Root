using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Email {
    public EmailData data;
    [HideInInspector] public bool markedAsRead;
    [HideInInspector] public string ipv6;
}

[System.Serializable]
public class EmailDifficulty {
    public enum Difficulty {
        EASY = 0b0000_0001,
        NORMAL = 0b0000_0010,
    }

    [Header("Easy")]
    [SerializeField] private Email[] easy;

    public Email GetEmail(Difficulty difficulty) {
        switch (difficulty) {
            case Difficulty.EASY:
                return this.easy[Random.Range(0, this.easy.Length)];
            default:
                return null;
        }
    }
}

public class EmailGenerator : MonoBehaviour {
    public enum EmailGeneratorType {
        ANY = HACKGRID | MATRIXCYPER,
        HACKGRID = 0b0000_0001,
        MATRIXCYPER = 0b0000_0010
    }

    public EmailDifficulty hackGrids;

    public Email GetEmail(EmailGeneratorType type, EmailDifficulty.Difficulty difficulty) {
        Email fetchedMail = null;
        switch (type) {
            case EmailGeneratorType.HACKGRID:
                fetchedMail = this.hackGrids.GetEmail(difficulty);
                break;
            default:
                return null;
        }
        fetchedMail.ipv6 = GenerateHash(fetchedMail.data.subject);
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
