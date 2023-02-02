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

    private void Start() {
        this.remainingKeyCodes = new Bag<string>();
        this.hexCodes = new Bag<string>();
        Initialize();
    }

    public void Initialize() {
        this.selector.Initialize(this.size, this.movementOffset, this.offset, ChoosePosition);
        this.hexData = new Grid<GameObject>(this.size.x, this.size.y);
        for (int y = 0; y < this.hexData.y; y++) {
            for (int x = 0; x < this.hexData.x; x++) {
                this.hexData[x, y] = Instantiate(
                    hexElementPrefab,
                    new Vector3(x * this.movementOffset + this.offset.x, y * this.movementOffset + this.offset.y, 0),
                    Quaternion.identity,
                    transform
                );
                string hexCode = this.hexData[x, y].GetComponent<HexElement>().Initialize();
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
        for (int y = 0; y < this.hexData.y; y++) {
            ClearRowIfPossible(y);
        }
        UpdateBank();
    }

    public void ChoosePosition(Vector2Int coord) {
        if (this.hexData[coord.x, coord.y] == null) return;
        if (this.hexData[coord.x, coord.y].GetComponent<HexElement>().isDestroyed) return;
        string hexCode = this.hexData[coord.x, coord.y].GetComponent<HexElement>().HexStr;
        if (this.remainingKeyCodes.ContainsKey(hexCode)) {
            this.remainingKeyCodes.Remove(hexCode);
            this.hexData[coord.x, coord.y].GetComponent<HexElement>().Destroy();
            ClearRowIfPossible(coord.y);
            UpdateBank();
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
    }
}
