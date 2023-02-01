using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackGrid : MonoBehaviour {
    public Vector2Int size;
    public HackSelector selector;

    private Grid<string> hexData;
    private string[] keyPhrase;

    private void Start() {
        Initialize();
    }

    public void Initialize() {
        this.selector.Initialize(this.size);
        this.hexData = new Grid<string>(this.size.x, this.size.y);
    }
}
