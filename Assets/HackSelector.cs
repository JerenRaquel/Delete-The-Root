using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackSelector : MonoBehaviour {
    public float movementOffset;
    public Vector2Int position;

    private Vector2Int maxPosition;
    private InputActions inputActions;

    private void Awake() {
        this.inputActions = new InputActions();
    }

    private void OnEnable() {
        this.inputActions.Enable();
        this.inputActions.HackGrid.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    private void OnDisable() {
        this.inputActions.Disable();
        this.inputActions.HackGrid.Movement.performed -= ctx => Move(ctx.ReadValue<Vector2>());
    }

    public void Initialize(Vector2Int maxPos) {
        this.maxPosition = maxPos;
        this.position.x = Mathf.FloorToInt(maxPos.x / 2);
        this.position.y = Mathf.FloorToInt(maxPos.y / 2);
    }

    private void Move(Vector2 direction) {
        if (direction == Vector2.zero) return;
        position.x = Mathf.FloorToInt(Mathf.Clamp(position.x + direction.x, 0, this.maxPosition.x));
        position.y = Mathf.FloorToInt(Mathf.Clamp(position.y + direction.y, 0, this.maxPosition.y));
        transform.position = new Vector3(this.position.x * movementOffset, this.position.y * movementOffset, 0);
    }
}
