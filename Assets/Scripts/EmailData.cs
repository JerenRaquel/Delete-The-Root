using UnityEngine;

[CreateAssetMenu(fileName = "EmailData", menuName = "Delete The Root/EmailData", order = 0)]
public class EmailData : ScriptableObject {
    public string subject;
    public string sender;
    [TextArea] public string message;

    public HackRewardData reward;
}