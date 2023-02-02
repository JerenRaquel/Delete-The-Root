using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexElement : MonoBehaviour {
    public string HexStr { get; private set; }
    public bool isDestroyed { get; private set; } = false;

    private static char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F' };
    private TMPro.TextMeshPro textBox;

    private void Awake() {
        this.textBox = gameObject.GetComponent<TMPro.TextMeshPro>();
    }

    public string Initialize(string forcedHexStr = null) {
        if (forcedHexStr == null) {
            int num = Random.Range(1, 10);
            char letter = letters[Random.Range(0, letters.Length)];
            this.HexStr = num.ToString() + letter;
        } else {
            this.HexStr = forcedHexStr;
        }
        this.textBox.text = this.HexStr;
        return this.HexStr;
    }

    public void Destroy() {
        this.HexStr = "....";
        this.textBox.text = this.HexStr;
        this.textBox.alignment = TMPro.TextAlignmentOptions.BottomFlush;
    }
}
