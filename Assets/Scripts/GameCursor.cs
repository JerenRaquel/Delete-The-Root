using UnityEngine;

public class GameCursor : MonoBehaviour {
    public Vector3 offset;

    private void OnEnable() {
        Cursor.visible = false;
    }

    private void OnDisable() {
        Cursor.visible = true;
    }

    void Update() {
        transform.position = Input.mousePosition + offset;
    }
}
