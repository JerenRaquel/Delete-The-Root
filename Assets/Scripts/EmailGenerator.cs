using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Email {
    public EmailData data;
    [HideInInspector] public bool markedAsRead;
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
        switch (type) {
            case EmailGeneratorType.HACKGRID:
                return this.hackGrids.GetEmail(difficulty);
            default:
                return null;
        }
    }
}
