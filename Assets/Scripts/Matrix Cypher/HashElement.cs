using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashElement : MonoBehaviour {
    private TMPro.TextMeshPro textBox;
    private BoxCollider2D collider2d;
    private Vector3 playerResetPosition;

    private void Awake() {
        this.textBox = gameObject.GetComponent<TMPro.TextMeshPro>();
        this.collider2d = gameObject.GetComponent<BoxCollider2D>();
    }

    public void Initialize(string hashString, Vector3 playerResetPosition) {
        this.textBox.text = hashString;
        this.playerResetPosition = playerResetPosition;
        this.collider2d.size = new Vector3(
            this.textBox.rectTransform.rect.width,
            this.textBox.rectTransform.rect.height,
            1
        );
        this.collider2d.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.gameObject.transform.position = this.playerResetPosition;
            // GameController.instance.RaiseAlert();
        }
    }
}
