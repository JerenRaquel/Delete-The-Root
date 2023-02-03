using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Delete The Root/UpgradeData", order = 0)]
public class UpgradeData : ScriptableObject {
    public new string name;
    public Sprite icon;
    public int cost;
}
