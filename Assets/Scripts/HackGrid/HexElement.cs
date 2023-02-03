using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexElement : MonoBehaviour {

    public string HexStr { get; private set; }
    public bool isDestroyed { get; private set; } = false;

    [SerializeField] private Color strikeColor;
    [SerializeField] private Color highlightedColor;

    private static char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F' };
    private TMPro.TextMeshPro textBox;

    private void Awake() {
        this.textBox = gameObject.GetComponent<TMPro.TextMeshPro>();
    }

    public string Initialize() {
        int num = Random.Range(1, 10);
        char letter = letters[Random.Range(0, letters.Length)];
        this.HexStr = num.ToString() + letter;
        this.textBox.text = this.HexStr;
        return this.HexStr;
    }

    public void Destroy() {
        this.HexStr = "....";
        this.textBox.text = this.HexStr;
        this.textBox.alignment = TMPro.TextAlignmentOptions.BottomFlush;
    }

    public void Highlight() {
        this.textBox.color = highlightedColor;
    }

    public void StrikeThrough() {
        this.textBox.color = strikeColor;
    }
}
