using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed;

    private InputActions inputActions;
    private Vector2 direction;

    private void Awake() {
        this.inputActions = new InputActions();
    }

    private void OnEnable() {
        this.inputActions.Enable();
        this.inputActions.HackGrid.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
        this.inputActions.HackGrid.Movement.canceled += _ => Idle();
    }

    private void OnDisable() {
        this.inputActions.Disable();
        this.inputActions.HackGrid.Movement.performed -= ctx => Move(ctx.ReadValue<Vector2>());
        this.inputActions.HackGrid.Movement.canceled -= _ => Idle();
    }

    private void FixedUpdate() {
        transform.position = transform.position + new Vector3(
            direction.x * this.speed * Time.deltaTime,
            direction.y * this.speed * Time.deltaTime,
            0
        );
    }

    private void Move(Vector2 direction) {
        if (direction == Vector2.zero) return;
        this.direction = direction;
    }

    private void Idle() {
        this.direction = Vector2.zero;
    }
}
