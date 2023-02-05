using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackGrid : MonoBehaviour {
    public Vector2Int size;
    public Vector2 offset;
    public float movementOffset;
    public HackSelector selector;
    public GameObject hexElementPrefab;
    public TMPro.TextMeshProUGUI bank;
    public int maxKeys;

    private Grid<GameObject> hexData;   // Hex Element
    private Bag<string> hexCodes;
    private Bag<string> remainingKeyCodes;
    private HashSet<Vector2Int> failedHexCodes;

    private void Start() {
        this.failedHexCodes = new HashSet<Vector2Int>();
        this.remainingKeyCodes = new Bag<string>();
        this.hexCodes = new Bag<string>();
        Initialize();
        Cursor.visible = false;
    }

    public void Initialize() {
        int level = AlertManager.instance.GetAlertLevel(
            GameController.instance.CurrentConnectionIPv6
        );
        Vector2Int newSize = this.size;
        Vector2 newOffset = this.offset;
        if (level > 3) {
            newSize.x += 2;
            newOffset.x -= 1;
        }
        if (level > 5) {
            newSize.x += 2;
            newOffset.x -= 1;
        }

        this.selector.Initialize(
            newSize,
            this.movementOffset,
            newOffset,
            ChoosePosition
        );
        this.hexData = new Grid<GameObject>(newSize.x, newSize.y);
        for (int y = 0; y < this.hexData.y; y++) {
            for (int x = 0; x < this.hexData.x; x++) {
                this.hexData[x, y] = Instantiate(
                    hexElementPrefab,
                    new Vector3(x * this.movementOffset + newOffset.x, y * this.movementOffset + newOffset.y, -5),
                    Quaternion.identity,
                    transform
                );
                string hexCode = this.hexData[x, y]
                    .GetComponent<HexElement>()
                    .Initialize();
                if (this.hexCodes.ContainsKey(hexCode)) {
                    this.hexCodes[hexCode]++;
                } else {
                    this.hexCodes.Add(hexCode);
                }
            }
        }
        int keyCodes = Mathf.Clamp(this.hexCodes.ItemCount - 3, 1, Mathf.Min(this.hexCodes.ItemCount, this.maxKeys));
        int count = 0;
        while (count < keyCodes) {
            string randomHexCode = this.hexCodes.PickRandomItem();
            if (!this.remainingKeyCodes.ContainsKey(randomHexCode)) {
                this.remainingKeyCodes.Add(randomHexCode, this.hexCodes[randomHexCode]);
                count++;
            }
        }

        bool enableHighlighting = PlayerProfiler.instance.IsEquiped("Syntax Highlighter");
        if (enableHighlighting) {
            for (int y = 0; y < this.hexData.y; y++) {
                for (int x = 0; x < this.hexData.x; x++) {
                    if (this.remainingKeyCodes
                        .ContainsKey(this.hexData[x, y]
                        .GetComponent<HexElement>().HexStr)
                        ) {
                        this.hexData[x, y].GetComponent<HexElement>().Highlight();
                    }
                }
            }
        }
        for (int y = 0; y < this.hexData.y; y++) {
            ClearRowIfPossible(y);
        }
        UpdateBank();
    }

    public void ChoosePosition(Vector2Int coord) {
        if (this.hexData[coord.x, coord.y] == null) return;
        if (this.hexData[coord.x, coord.y].GetComponent<HexElement>().isDestroyed) return;
        string hexCode = this.hexData[coord.x, coord.y].GetComponent<HexElement>().HexStr;
        if (hexCode == "....") return;
        if (this.failedHexCodes.Contains(coord)) return;

        if (this.remainingKeyCodes.ContainsKey(hexCode)) {
            this.remainingKeyCodes.Remove(hexCode);
            this.hexData[coord.x, coord.y].GetComponent<HexElement>().Destroy();
            ClearRowIfPossible(coord.y);
            UpdateBank();
        } else {
            this.failedHexCodes.Add(coord);
            AlertManager.instance.RaiseAlertLevel(GameController.instance.CurrentConnectionIPv6);
            this.hexData[coord.x, coord.y].GetComponent<HexElement>().StrikeThrough();
            // Play Animation
        }
    }

    private void UpdateBank() {
        string text = "";
        int count = 0;
        int max = this.remainingKeyCodes.ItemCount;
        this.remainingKeyCodes.Foreach((in string hexCode, in int amount) => {
            text += hexCode + ":" + amount.ToString();
            if (count < max) {
                text += " ";
            }
            count++;
        });
        this.bank.text = text;
    }

    private void ClearRowIfPossible(int y) {
        int count = 0;
        for (int x = 0; x < this.hexData.x; x++) {
            string hexStr = this.hexData[x, y].GetComponent<HexElement>().HexStr;
            if (!this.remainingKeyCodes.ContainsKey(hexStr)) count++;
        }
        if (count >= this.hexData.x) {
            for (int x = 0; x < this.hexData.x; x++) {
                this.hexData[x, y].GetComponent<HexElement>().Destroy();
            }
        }
        if (this.remainingKeyCodes.ItemCount == 0) {
            Cursor.visible = true;
            GameController.instance.HackComplete();
        }
    }
}
