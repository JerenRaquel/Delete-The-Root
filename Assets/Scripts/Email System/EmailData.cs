using UnityEngine;

[CreateAssetMenu(fileName = "EmailData", menuName = "Delete The Root/EmailData", order = 0)]
public class EmailData : ScriptableObject {
    [Header("Email Content")]
    public string subject;
    public string sender;
    [TextArea(15, 20)] public string message;

    [Header("Game Data")]
    public int level;
    public EmailGenerator.EmailGeneratorType hackingType;
    public HackRewardData reward;

    [Header("Directory Data")]
    public DirectoryScriptable rootFolder;
}