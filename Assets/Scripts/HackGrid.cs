using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackGrid : MonoBehaviour {
    public Vector2Int size;
    public float movementOffset;
    public HackSelector selector;
    public GameObject hexElementPrefab;

    private Grid<GameObject> hexData;   // Hex Element
    private string[] keyPhrase;

    private void Start() {
        Initialize();
    }

    public void Initialize() {
        this.selector.Initialize(this.size, this.movementOffset);
        this.hexData = new Grid<GameObject>(this.size.x, this.size.y);
        for (int y = 0; y < this.hexData.y; y++) {
            for (int x = 0; x < this.hexData.x; x++) {
                this.hexData[x, y] = Instantiate(
                    hexElementPrefab,
                    new Vector3(x * this.movementOffset, y * this.movementOffset, 0),
                    Quaternion.identity,
                    transform
                );
                this.hexData[x, y].GetComponent<HexElement>().Initialize();
            }
        }
    }
}
