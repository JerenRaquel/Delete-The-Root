using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexElement : MonoBehaviour {
    public string HexStr { get; private set; }

    private static char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F' };

    public void Initialize(string forcedHexStr = null) {
        if (forcedHexStr == null) {
            int num = Random.Range(1, 10);
            char letter = letters[Random.Range(0, letters.Length)];
        } else {
            this.HexStr = forcedHexStr;
        }
    }
}
