using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HackSelector : MonoBehaviour {
    public delegate void PositionCallback(Vector2Int position);

    public Vector2Int position;

    private Vector2Int maxPosition;
    private InputActions inputActions;
    private float movementOffset;
    private Vector2 offset;
    private PositionCallback callback;

    private void Awake() {
        this.inputActions = new InputActions();
    }

    private void OnEnable() {
        this.inputActions.Enable();
        this.inputActions.HackGrid.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
        this.inputActions.HackGrid.Select.performed += _ => Select();
    }

    private void OnDisable() {
        this.inputActions.Disable();
        this.inputActions.HackGrid.Movement.performed -= ctx => Move(ctx.ReadValue<Vector2>());
        this.inputActions.HackGrid.Select.performed -= _ => Select();
    }

    public void Initialize(Vector2Int maxPos, float offset, Vector2 gridOffset, PositionCallback callback) {
        this.maxPosition = maxPos;
        this.position.x = Mathf.FloorToInt(maxPos.x / 2);
        this.position.y = Mathf.FloorToInt(maxPos.y / 2);
        this.movementOffset = offset;
        this.offset = gridOffset;
        this.callback = callback;
        transform.position = new Vector3(this.position.x * movementOffset + this.offset.x, this.position.y * movementOffset + this.offset.y, 0);
    }

    private void Move(Vector2 direction) {
        if (GameController.instance.ActiveTutorialWorking) return;
        if (direction == Vector2.zero) return;
        position.x = Mathf.FloorToInt(Mathf.Clamp(position.x + direction.x, 0, this.maxPosition.x - 1));
        position.y = Mathf.FloorToInt(Mathf.Clamp(position.y + direction.y, 0, this.maxPosition.y - 1));
        transform.position = new Vector3(this.position.x * movementOffset + this.offset.x, this.position.y * movementOffset + this.offset.y, 0);
    }

    private void Select() {
        if (GameController.instance.ActiveTutorialWorking) return;
        this.callback(this.position);
    }
}
