using UnityEngine;

public class GameCursor : MonoBehaviour {
    public Vector3 offset;
    public bool useWorldCoords = false;

    private void OnEnable() {
        Cursor.visible = false;
    }

    private void OnDisable() {
        Cursor.visible = true;
    }

    void Update() {
        if (this.useWorldCoords) {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        } else {
            transform.position = Input.mousePosition + offset;
        }
    }
}
