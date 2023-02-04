using UnityEngine;


[CreateAssetMenu(fileName = "DirectoryScriptable", menuName = "Delete The Root/DirectoryScriptable", order = 0)]
public class DirectoryScriptable : ScriptableObject {
    public string root;
    public DirectoryData[] directoryData;
}